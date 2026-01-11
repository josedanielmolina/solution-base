using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;
using Core.Application.Features.Auth;

namespace Core.Application.Facades;

public class AuthFacade : IAuthFacade
{
    private readonly Login _login;

    public AuthFacade(Login login)
    {
        _login = login;
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        return await _login.ExecuteAsync(dto);
    }
}
