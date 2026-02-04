using API.Extensions;
using API.Authorization;
using Core.Application.DTOs.Roles;
using Core.Application.Features.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly GetRoles _getRoles;
    private readonly GetRoleWithPermissions _getRoleWithPermissions;
    private readonly UpdateRolePermissions _updateRolePermissions;

    public RolesController(
        GetRoles getRoles,
        GetRoleWithPermissions getRoleWithPermissions,
        UpdateRolePermissions updateRolePermissions)
    {
        _getRoles = getRoles;
        _getRoleWithPermissions = getRoleWithPermissions;
        _updateRolePermissions = updateRolePermissions;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    [HttpGet]
    [HasPermission("roles.view")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getRoles.ExecuteAsync();
        return result.ToActionResult();
    }

    /// <summary>
    /// Get role with its permissions
    /// </summary>
    [HttpGet("{id}")]
    [HasPermission("roles.view")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _getRoleWithPermissions.ExecuteAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update role permissions
    /// </summary>
    [HttpPut("{id}/permissions")]
    [HasPermission("roles.configure")]
    public async Task<IActionResult> UpdatePermissions(int id, [FromBody] UpdateRolePermissionsDto dto)
    {
        var result = await _updateRolePermissions.ExecuteAsync(id, dto);
        return result.ToActionResult();
    }
}
