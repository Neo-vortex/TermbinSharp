using System.Security.Cryptography;
using System.Text;
using Mediator;
using Microsoft.EntityFrameworkCore;
using TermbinSharp.Models;

namespace TermbinSharp.Services.Queries.Handlers;

public class GetQueryHandler : IRequestHandler<GetQuery, RequestResult<string>>
{
    private static readonly Func<ApplicationDbContext, string, Task<Data?>> FirstOrDefaultCompiledQuery =
        EF.CompileAsyncQuery((ApplicationDbContext _appDbContext, string url) =>
            _appDbContext.Data
                .AsNoTracking()
                .FirstOrDefault(n => n.URL == url));

    private readonly ApplicationDbContext _applicationDbContext;

    public GetQueryHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async ValueTask<RequestResult<string>> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var value = await FirstOrDefaultCompiledQuery(_applicationDbContext, request.RequestedUrl);
            if (value == null) return new Exception("No data found");

            var dataValid = SHA512.HashData(Encoding.UTF8.GetBytes(value.DataString)).SequenceEqual(value.Checksum);
            if (!dataValid) return new Exception("Data is not valid");

            return value.DataString;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}