using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Users;
using Core.Application.Interfaces;
using Core.Application.Mappings;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Operations.Users;

public class CreateUserOperation
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserOperation(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserDto>> ExecuteAsync(CreateUserDto dto)
    {
        // Check if email already exists
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return Result.Failure<UserDto>(UserErrors.EmailAlreadyExists(dto.Email));
        }

        // Hash password
        var passwordHash = _passwordHasher.HashPassword(dto.Password);

        // Create user entity
        var user = dto.ToEntity(passwordHash);

        // Save to database
        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(user.ToDto());
    }
}
