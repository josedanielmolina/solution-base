using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Users;
using Core.Application.Mappings;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Users;

public class UpdateProfile
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfile(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> ExecuteAsync(int userId, UpdateProfileDto dto)
    {
        var user = await _userRepository.GetWithRolesAsync(userId);
        if (user == null)
        {
            return Result.Failure<UserDto>(UserErrors.NotFound(userId));
        }

        dto.UpdateProfileEntity(user);
        
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(user.ToDto());
    }
}
