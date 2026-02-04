using API.Extensions;
using Core.Application.DTOs.Auth;
using Core.Application.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Login _login;
    private readonly ChangePassword _changePassword;
    private readonly RequestPasswordReset _requestPasswordReset;
    private readonly ResetPassword _resetPassword;
    private readonly GetCurrentUser _getCurrentUser;

    public AuthController(
        Login login,
        ChangePassword changePassword,
        RequestPasswordReset requestPasswordReset,
        ResetPassword resetPassword,
        GetCurrentUser getCurrentUser)
    {
        _login = login;
        _changePassword = changePassword;
        _requestPasswordReset = requestPasswordReset;
        _resetPassword = resetPassword;
        _getCurrentUser = getCurrentUser;
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _login.ExecuteAsync(dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Change password for authenticated user
    /// </summary>
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = GetUserId();
        var result = await _changePassword.ExecuteAsync(userId, dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Request password reset code
    /// </summary>
    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto dto)
    {
        var result = await _requestPasswordReset.ExecuteAsync(dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Reset password with code
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var result = await _resetPassword.ExecuteAsync(dto);
        return result.ToActionResult();
    }

    /// <summary>
    /// Get current authenticated user
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetUserId();
        var result = await _getCurrentUser.ExecuteAsync(userId);
        return result.ToActionResult();
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) 
            ?? User.FindFirst("sub");
        return int.Parse(userIdClaim?.Value ?? "0");
    }
}

