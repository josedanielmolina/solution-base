using Core.Application.DTOs.Users;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Users;

public class SearchUsers
{
    private readonly IUserRepository _userRepository;

    public SearchUsers(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserSearchDto>> ExecuteAsync(string query, int limit = 5)
    {
        var users = await _userRepository.SearchAsync(query, limit);
        
        return users.Select(u => new UserSearchDto(
            u.Id,
            u.FirstName,
            u.LastName,
            u.Email,
            u.Document
        ));
    }
}
