using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;

namespace Core.Application.Facades;

public interface IAuthFacade
{
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
}
