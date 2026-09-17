namespace Eco.Application.Common.Results;

public class Result<T>
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public string? ErrorCode { get; init; }
    public T? Data { get; init; }

    public static Result<T> Ok(T data) => new() { Success = true, Data = data };

    public static Result<T> Fail(string errorCode, string message) =>
        new() { Success = false, ErrorCode = errorCode, ErrorMessage = message };
}