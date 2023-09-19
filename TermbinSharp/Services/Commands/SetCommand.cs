using Mediator;
using OneOf;
using TermbinSharp.Models;

namespace TermbinSharp.Services.Commands;

public class SetCommand : IRequest<RequestResult<string>>
{
    public SetCommand(string data)
    {
        Data = data;
    }

    public string Data { get; set; }
}