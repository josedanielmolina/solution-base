using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;
using Core.Application.Interfaces;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Auth;

public class ChangePassword
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePassword(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> ExecuteAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return Result.Failure(AuthErrors.UserNotFound);
        }

        // Verify current password
        if (!_passwordHasher.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
        {
            return Result.Failure(AuthErrors.InvalidCurrentPassword);
        }

        // Update password
        user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
        user.RequiresPasswordChange = false;
        
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}
