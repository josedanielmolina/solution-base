using API.Extensions;
using Core.Application.DTOs.Auth;
using Core.Application.Facades;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthFacade _authFacade;
    private readonly IValidator<LoginDto> _loginValidator;

    public AuthController(
        IAuthFacade authFacade,
        IValidator<LoginDto> loginValidator)
    {
        _authFacade = authFacade;
        _loginValidator = loginValidator;
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var validationResult = await _loginValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                error = "Validation.Failed",
                message = "One or more validation errors occurred.",
                details = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        }

        var result = await _authFacade.LoginAsync(dto);
        return result.ToActionResult();
    }
}
