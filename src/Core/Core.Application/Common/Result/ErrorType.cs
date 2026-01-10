namespace Core.Application.Common.Result;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    ServerError
}
