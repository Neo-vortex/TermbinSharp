using MediatR;
using OneOf;

namespace TermbinSharp.Services.Queries;

public class GetQuery : IRequest<OneOf<string,Exception>>
{
    public GetQuery(string requestedHash)
    {
        RequestedHash = requestedHash;
    }

    public string RequestedHash { get; set; }
    
}