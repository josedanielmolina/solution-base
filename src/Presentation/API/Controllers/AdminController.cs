using Core.Application.Features.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly SetPassword _setPassword;

    public AdminController(SetPassword setPassword)
    {
        _setPassword = setPassword;
    }

    /// <summary>
    /// Actualiza la contraseña de un usuario por email (solo desarrollo/seeding)
    /// </summary>
    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword([FromBody] SetPasswordDto request)
    {
        var result = await _setPassword.ExecuteAsync(request);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Error.Message });
        }

        return Ok(result.Value);
    }
}
