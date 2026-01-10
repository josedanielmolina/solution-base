namespace Core.Application.Common.Result;

public class Result<T> : Result
{
    public T? Value { get; }

    internal Result(T value, bool isSuccess, Error? error) 
        : base(isSuccess, error)
    {
        Value = value;
    }
}
