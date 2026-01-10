namespace Core.Application.Common.Errors;

public static class UserErrors
{
    public static Common.Result.Error NotFound(int id) =>
        Common.Result.Error.NotFound("User.NotFound", $"User with ID {id} was not found.");

    public static Common.Result.Error EmailAlreadyExists(string email) =>
        Common.Result.Error.Conflict("User.EmailExists", $"User with email '{email}' already exists.");

    public static Common.Result.Error InvalidCredentials() =>
        Common.Result.Error.Unauthorized("User.InvalidCredentials", "Invalid email or password.");

    public static Common.Result.Error EmailNotVerified() =>
        Common.Result.Error.Forbidden("User.EmailNotVerified", "Email is not verified.");

    public static Common.Result.Error InactiveUser() =>
        Common.Result.Error.Forbidden("User.Inactive", "User account is inactive.");
}
