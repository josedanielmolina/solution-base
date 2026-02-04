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

    public static Error InvalidCurrentPassword => Error.Validation(
        "Auth.InvalidCurrentPassword",
        "Current password is incorrect");

    public static Error InvalidResetCode => Error.Validation(
        "Auth.InvalidResetCode",
        "Invalid or expired reset code");

    public static Error UserNotFound => Error.NotFound(
        "Auth.UserNotFound",
        "User not found");
}

