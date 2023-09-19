using System.Security.Cryptography;
using System.Text;
using MediatR;
using OneOf;

namespace TermbinSharp.Services.Commands.Handlers;

public class SetCommandHandler : IRequestHandler<SetCommand, OneOf<string, Exception>>
{
    public Task<OneOf<string, Exception>> Handle(SetCommand request, CancellationToken cancellationToken)
    {
        return Task.Run<OneOf<string, Exception>>(() =>
        {
            try
            {
                Span<byte> result = stackalloc byte[64];
                var checksumDone = SHA512.TryHashData(Encoding.UTF8.GetBytes(request.Data), result, out _);
                if (!checksumDone)
                {
                    return new Exception("checksum failed");
                }
                var checksum = Convert.ToHexString(result);
                SaveData(request, checksum);
                return checksum;
            }
            catch (Exception e)
            {
                return e;
            }
        }, cancellationToken);

    }

    private static void SaveData(SetCommand request, string checksum)
    {
        if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Data", checksum)))
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "Data", checksum),
                request.Data);
        }
    }
}