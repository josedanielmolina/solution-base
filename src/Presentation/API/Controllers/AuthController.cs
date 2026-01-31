using API.Extensions;
using Core.Application.DTOs.Auth;
using Core.Application.Features.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Login _login;

    public AuthController(Login login)
    {
        _login = login;
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // Validación automática por FluentValidationFilter
        var result = await _login.ExecuteAsync(dto);
        return result.ToActionResult();
    }
}
