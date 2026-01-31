using API.Extensions;
using Core.Application.DTOs.Users;
using Core.Application.Features.Users;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly CreateUser _createUser;
    private readonly GetUser _getUser;
    private readonly UpdateUser _updateUser;
    private readonly DeleteUser _deleteUser;

    public UsersController(
        CreateUser createUser,
        GetUser getUser,
        UpdateUser updateUser,
        DeleteUser deleteUser)
    {
        _createUser = createUser;
        _getUser = getUser;
        _updateUser = updateUser;
        _deleteUser = deleteUser;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _getUser.ExecuteAllAsync();
        return result.ToActionResult();
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _getUser.ExecuteAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        // Validación automática por FluentValidationFilter
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
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        // Validación automática por FluentValidationFilter
        var result = await _updateUser.ExecuteAsync(id, dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deleteUser.ExecuteAsync(id);
        return result.ToActionResult();
    }
}
