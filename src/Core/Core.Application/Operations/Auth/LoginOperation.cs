using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;
using Core.Application.Interfaces;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Operations.Auth;

public class LoginOperation
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public LoginOperation(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResponseDto>> ExecuteAsync(LoginDto dto)
    {
        // Find user by email
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
        {
            return Result.Failure<AuthResponseDto>(AuthErrors.InvalidCredentials);
        }

        // Verify password
        if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
        {
            return Result.Failure<AuthResponseDto>(AuthErrors.InvalidCredentials);
        }

        // Check if user is active
        if (!user.IsActive)
        {
            return Result.Failure<AuthResponseDto>(AuthErrors.UserNotActive);
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Generate JWT token
        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName);

        // Return response
        var response = new AuthResponseDto(
            token,
            new AuthUserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email
            )
        );

        return Result.Success(response);
    }
}
