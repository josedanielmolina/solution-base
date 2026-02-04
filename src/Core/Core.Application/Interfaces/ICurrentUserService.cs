namespace Core.Application.Interfaces;

/// <summary>
/// Interface to get current authenticated user information
/// </summary>
public interface ICurrentUserService
{
    int UserId { get; }
    string? Email { get; }
    IEnumerable<string>? Roles { get; }
    bool IsAuthenticated { get; }
}
