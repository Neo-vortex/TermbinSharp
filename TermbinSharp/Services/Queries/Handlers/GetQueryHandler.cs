using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using OneOf;

namespace TermbinSharp.Services.Queries.Handlers;

public class GetQueryHandler : IRequestHandler<GetQuery, OneOf<string, Exception>>
{
    

    public async Task<OneOf<string, Exception>> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Data", request.RequestedHash)))
            return new Exception("no such a data");

        var value = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "Data", request.RequestedHash),
            cancellationToken);
        var hashedResult = Convert.ToHexString(SHA512.HashData(Encoding.UTF8.GetBytes(value)));
        if (hashedResult != request.RequestedHash) throw new Exception("data is corrupted");

        return value;

    }
}