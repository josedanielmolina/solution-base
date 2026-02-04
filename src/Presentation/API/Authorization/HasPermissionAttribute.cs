using Microsoft.AspNetCore.Authorization;

namespace API.Authorization;

/// <summary>
/// Authorization attribute that checks for a specific permission claim
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) 
        : base(policy: permission)
    {
    }
}
