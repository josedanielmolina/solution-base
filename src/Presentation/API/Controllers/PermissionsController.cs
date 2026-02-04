using API.Extensions;
using API.Authorization;
using Core.Application.Features.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PermissionsController : ControllerBase
{
    private readonly GetPermissions _getPermissions;

    public PermissionsController(GetPermissions getPermissions)
    {
        _getPermissions = getPermissions;
    }

    /// <summary>
    /// Get all permissions
    /// </summary>
    [HttpGet]
    [HasPermission("roles.configure")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getPermissions.ExecuteAsync();
        return result.ToActionResult();
    }

    /// <summary>
    /// Get permissions by module
    /// </summary>
    [HttpGet("module/{module}")]
    [HasPermission("roles.configure")]
    public async Task<IActionResult> GetByModule(string module)
    {
        var result = await _getPermissions.ExecuteByModuleAsync(module);
        return result.ToActionResult();
    }
}
