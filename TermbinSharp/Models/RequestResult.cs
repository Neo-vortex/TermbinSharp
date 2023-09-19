using OneOf;

namespace TermbinSharp.Models;


[GenerateOneOf]
public  partial class RequestResult<T> : OneOfBase<T, Exception>
{

    public bool IsSuccess => this.IsT0;
    public bool IsFailure => this.IsT1;
    public T ActualValue => this.AsT0;
    public Exception Error => this.AsT1;

}