using System.Security.Cryptography;
using System.Text;
using Mediator;
using Microsoft.EntityFrameworkCore;
using TermbinSharp.Models;

namespace TermbinSharp.Services.Commands.Handlers;

public class SetCommandHandler : IRequestHandler<SetCommand, RequestResult<string>>
{
    private static readonly Func<ApplicationDbContext, string, Task<Data?>> FirstOrDefaultCompiledQuery =
        EF.CompileAsyncQuery((ApplicationDbContext _appDbContext, string data) =>
            _appDbContext.Data.AsNoTracking().FirstOrDefault(n => n.DataString == data));
    

    private readonly ApplicationDbContext _applicationDbContext;

    public SetCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async ValueTask<RequestResult<string>> Handle(SetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Data)) return new Exception("Data is empty");

            var data = await FirstOrDefaultCompiledQuery(_applicationDbContext, request.Data);

            if (data != null) return data.URL;

            var randomUrl = Path.GetRandomFileName().Replace(".", "");
            await _applicationDbContext.Data.AddAsync(new Data
            {
                DataString = request.Data,
                URL = randomUrl,
                Checksum = SHA512.HashData(Encoding.UTF8.GetBytes(request.Data))
            }, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return randomUrl;
        }
        catch (Exception e)
        {
            return e;
        }
    }
}