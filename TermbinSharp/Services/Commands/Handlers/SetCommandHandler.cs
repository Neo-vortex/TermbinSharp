using System.Security.Cryptography;
using Mediator;
using Microsoft.EntityFrameworkCore;
using OneOf;
using TermbinSharp.Models;

namespace TermbinSharp.Services.Commands.Handlers;

public class SetCommandHandler : IRequestHandler<SetCommand,RequestResult<string>>
{

    private readonly ApplicationDbContext _applicationDbContext;

    public SetCommandHandler(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async ValueTask <RequestResult<string>> Handle(SetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Data)) return new Exception("Data is empty");

            var data = await _applicationDbContext.Data.FirstOrDefaultAsync(data => data.DataString == request.Data, cancellationToken: cancellationToken);

            if (data != null)
            {
                return data.URL;
            }
            
            var randomUrl = Path.GetRandomFileName().Replace(".", "");
            await _applicationDbContext.Data.AddAsync(new Data()
            {
                DataString = request.Data,
                URL = randomUrl,
                Checksum = SHA512.HashData(System.Text.Encoding.UTF8.GetBytes(request.Data))
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