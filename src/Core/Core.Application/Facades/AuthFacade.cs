using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;
using Core.Application.Operations.Auth;

namespace Core.Application.Facades;

public class AuthFacade : IAuthFacade
{
    private readonly LoginOperation _loginOperation;

    public AuthFacade(LoginOperation loginOperation)
    {
        _loginOperation = loginOperation;
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        return await _loginOperation.ExecuteAsync(dto);
    }
}
