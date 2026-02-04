using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.Interfaces;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Auth;

public record SetPasswordDto(string Email, string NewPassword);
public record SetPasswordResultDto(bool Success, string Message);

public class SetPassword
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public SetPassword(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SetPasswordResultDto>> ExecuteAsync(SetPasswordDto dto)
    {
        // Find user by email
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            return Result.Failure<SetPasswordResultDto>(AuthErrors.UserNotFound);
        }

        // Hash and set new password
        user.PasswordHash = _passwordHasher.HashPassword(dto.NewPassword);
        user.RequiresPasswordChange = false;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(new SetPasswordResultDto(true, $"Contraseña actualizada para {dto.Email}"));
    }
}
