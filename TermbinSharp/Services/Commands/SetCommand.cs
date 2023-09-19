using MediatR;
using OneOf;

namespace TermbinSharp.Services.Commands;

public class SetCommand : IRequest<OneOf<string, Exception>>
{
    public SetCommand(string data)
    {
        Data = data;
    }

    public string Data { get; set; }
}