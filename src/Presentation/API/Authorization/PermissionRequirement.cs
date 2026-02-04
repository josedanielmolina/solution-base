using Microsoft.AspNetCore.Authorization;

namespace API.Authorization;

/// <summary>
/// Authorization requirement that requires a specific permission
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}
