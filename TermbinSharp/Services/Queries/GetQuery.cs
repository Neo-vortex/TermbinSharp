using Mediator;
using OneOf;
using TermbinSharp.Models;

namespace TermbinSharp.Services.Queries;

public class GetQuery : IRequest<RequestResult<string>>
{
    public GetQuery(string requestedHash)
    {
        RequestedUrl = requestedHash;
    }

    public string RequestedUrl { get; set; }
    
}