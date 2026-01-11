using Core.Application.Common.Result;

namespace Core.Application.Common.Errors;

public static class AuthErrors
{
    public static Error InvalidCredentials => Error.Unauthorized(
        "Auth.InvalidCredentials",
        "Invalid email or password");

    public static Error UserNotActive => Error.Unauthorized(
        "Auth.UserNotActive",
        "User account is not active");

    public static Error EmailNotVerified => Error.Unauthorized(
        "Auth.EmailNotVerified",
        "Email address is not verified");
}
