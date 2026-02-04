using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;
using Core.Application.Interfaces;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Auth;

public class Login
{
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public Login(
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResponseDto>> ExecuteAsync(LoginDto dto)
    {
        // Find user by email with roles
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

        // Get user with roles and permissions
        var userWithRoles = await _userRepository.GetWithRolesAndPermissionsAsync(user.Id);
        var roles = userWithRoles?.UserRoles.Select(ur => ur.Role.Name).ToList() ?? new List<string>();
        var permissions = await _permissionRepository.GetCodesByUserIdAsync(user.Id);

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Generate JWT token with roles and permissions
        var token = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            roles,
            permissions);

        // Return response
        var response = new AuthResponseDto(
            token,
            new AuthUserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                roles,
                permissions
            ),
            user.RequiresPasswordChange
        );

        return Result.Success(response);
    }
}

