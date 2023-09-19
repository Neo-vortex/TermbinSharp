using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TermbinSharp.Models;

namespace TermbinSharp.Services.Queries.Handlers;

public class GetQueryHandler : IRequestHandler<GetQuery, RequestResult<string>>
{
    private readonly ApplicationDbContext _applicationDbContext;

    
    private static Func<ApplicationDbContext, string, Task<Data?>> FirstOrDefault_CompiledQuery =
        EF.CompileAsyncQuery((ApplicationDbContext _appDbContext, string url) => _appDbContext.Data.FirstOrDefault(n => n.URL == url));

    public GetQueryHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async  ValueTask <RequestResult<string>> Handle(GetQuery request, CancellationToken cancellationToken)
    {


        try
        {
            
            //var value = await _applicationDbContext.Data.FirstOrDefaultAsync(data => data.URL == request.RequestedUrl, CancellationToken.None);
            var value =  await FirstOrDefault_CompiledQuery(_applicationDbContext, request.RequestedUrl);
            if (value == null) return new Exception("No data found");

            var dataValid = SHA512.HashData(Encoding.UTF8.GetBytes(value.DataString)).SequenceEqual(value.Checksum) ;
            if (!dataValid) return new Exception("Data is not valid");

            return value.DataString;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}