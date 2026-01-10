using API.Extensions;
using API.Filters;
using Core.Application.DTOs.Users;
using Core.Application.Facades;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserFacade _userFacade;
    private readonly IValidator<CreateUserDto> _createUserValidator;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;

    public UsersController(
        IUserFacade userFacade,
        IValidator<CreateUserDto> createUserValidator,
        IValidator<UpdateUserDto> updateUserValidator)
    {
        _userFacade = userFacade;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
    }

    /// <summary>
    /// Get all users
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userFacade.GetAllUsersAsync();
        return result.ToActionResult();
    }

    /// <summary>
    /// Get user by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userFacade.GetUserByIdAsync(id);
        return result.ToActionResult();
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var validationResult = await _createUserValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                error = "Validation.Failed",
                message = "One or more validation errors occurred.",
                details = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        }

        var result = await _userFacade.CreateUserAsync(dto);
        
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
    [ValidateModel]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var validationResult = await _updateUserValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                error = "Validation.Failed",
                message = "One or more validation errors occurred.",
                details = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        }

        var result = await _userFacade.UpdateUserAsync(id, dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _userFacade.DeleteUserAsync(id);
        return result.ToActionResult();
    }
}
