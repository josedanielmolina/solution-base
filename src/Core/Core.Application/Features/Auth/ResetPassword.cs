using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;
using Core.Application.Interfaces;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Auth;

public class ResetPassword
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ResetPassword(
        IUserRepository userRepository,
        IPasswordResetTokenRepository tokenRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> ExecuteAsync(ResetPasswordDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            return Result.Failure(AuthErrors.InvalidResetCode);
        }

        // Validate token
        var token = await _tokenRepository.GetValidTokenAsync(user.Id, dto.Code);
        if (token == null)
        {
            return Result.Failure(AuthErrors.InvalidResetCode);
        }

        // Mark token as used
        token.IsUsed = true;
        await _tokenRepository.UpdateAsync(token);

        // Update password
        user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
        user.RequiresPasswordChange = false;
        
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
