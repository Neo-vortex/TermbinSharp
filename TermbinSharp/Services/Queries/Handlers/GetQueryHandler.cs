using System.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using OneOf;

namespace TermbinSharp.Services.Queries.Handlers;

public class GetQueryHandler : IRequestHandler<GetQuery , OneOf<string,Exception>>
{
    private readonly IMemoryCache _memoryCache;

    private static readonly MemoryCacheEntryOptions CacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromHours(1));

    public GetQueryHandler(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<OneOf<string, Exception>> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        if (!_memoryCache.TryGetValue(request.RequestedHash, out string cacheValue))
        {
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Data", request.RequestedHash)))
            {
                return new Exception("no such a data");
            }

            cacheValue = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "Data", request.RequestedHash), cancellationToken);
            _memoryCache.Set(request.RequestedHash, cacheValue, CacheEntryOptions);

            var hashedResult = Convert.ToHexString(SHA512.HashData(System.Text.Encoding.UTF8.GetBytes(cacheValue)));

            if (hashedResult != request.RequestedHash)
            {
                throw new Exception("data is corrupted");
            }
            return cacheValue;
        }
        else
        {
            return cacheValue;
        }
    }
}