using OneOf;

namespace TermbinSharp.Models;

[GenerateOneOf]
public partial class RequestResult<T> : OneOfBase<T, Exception>
{
    public bool IsSuccess => IsT0;
    public bool IsFailure => IsT1;
    public T ActualValue => AsT0;
    public Exception Error => AsT1;
}