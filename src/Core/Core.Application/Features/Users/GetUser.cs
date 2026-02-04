using Core.Application.Common.Errors;
using Core.Application.Common.Result;
using Core.Application.DTOs.Users;
using Core.Application.Mappings;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Users;

public class GetUser
{
    private readonly IUserRepository _userRepository;

    public GetUser(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> ExecuteAsync(int id)
    {
        var user = await _userRepository.GetWithRolesAsync(id);

        if (user == null)
        {
            return Result.Failure<UserDto>(UserErrors.NotFound(id));
        }

        return Result.Success(user.ToDto());
    }

    public async Task<Result<IEnumerable<UserDto>>> ExecuteAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var userDtos = users.Select(u => u.ToDto());
        
        return Result.Success(userDtos);
    }
}
