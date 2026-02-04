using API.Extensions;
using API.Authorization;
using Core.Application.DTOs.Users;
using Core.Application.Features.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly CreateUser _createUser;
    private readonly GetUser _getUser;
    private readonly UpdateUser _updateUser;
    private readonly DeleteUser _deleteUser;
    private readonly UpdateProfile _updateProfile;
    private readonly AssignUserRoles _assignUserRoles;
    private readonly SearchUsers _searchUsers;

    public UsersController(
        CreateUser createUser,
        GetUser getUser,
        UpdateUser updateUser,
        DeleteUser deleteUser,
        UpdateProfile updateProfile,
        AssignUserRoles assignUserRoles,
        SearchUsers searchUsers)
    {
        _createUser = createUser;
        _getUser = getUser;
        _updateUser = updateUser;
        _deleteUser = deleteUser;
        _updateProfile = updateProfile;
        _assignUserRoles = assignUserRoles;
        _searchUsers = searchUsers;
    }

    /// <summary>
    /// Search users by name, email, or document
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int limit = 5)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            return Ok(Array.Empty<object>());
        
        var results = await _searchUsers.ExecuteAsync(q, Math.Min(limit, 10));
        return Ok(results);
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    [HasPermission("users.view")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getUser.ExecuteAllAsync();
        return result.ToActionResult();
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    [HttpGet("{id}")]
    [HasPermission("users.view")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _getUser.ExecuteAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [HasPermission("users.create")]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var result = await _createUser.ExecuteAsync(dto);
        
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
        }

        return result.ToActionResult();
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    [HttpPut("{id}")]
    [HasPermission("users.edit")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var result = await _updateUser.ExecuteAsync(id, dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id}")]
    [HasPermission("users.delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteUser.ExecuteAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Update current user's profile
    /// </summary>
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = GetUserId();
        var result = await _updateProfile.ExecuteAsync(userId, dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Assign roles to a user
    /// </summary>
    [HttpPut("{id}/roles")]
    [HasPermission("users.assign-roles")]
    public async Task<IActionResult> AssignRoles(int id, [FromBody] AssignUserRolesDto dto)
    {
        var result = await _assignUserRoles.ExecuteAsync(id, dto);
        return result.ToActionResult();
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        return int.Parse(userIdClaim?.Value ?? "0");
    }
}

