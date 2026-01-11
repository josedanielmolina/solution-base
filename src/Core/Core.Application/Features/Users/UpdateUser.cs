using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Users;
using Core.Application.Mappings;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Users;

public class UpdateUser
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public UpdateUser(
        IUnitOfWork unitOfWork,
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> ExecuteAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return Result.Failure<UserDto>(UserErrors.NotFound(id));
        }

        dto.UpdateEntity(user);
        
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(user.ToDto());
    }
}
