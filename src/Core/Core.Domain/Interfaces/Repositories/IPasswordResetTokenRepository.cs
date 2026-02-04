using Core.Domain.Entities;

namespace Core.Domain.Interfaces.Repositories;

public interface IPasswordResetTokenRepository
{
    Task<PasswordResetToken?> GetByIdAsync(int id);
    Task<PasswordResetToken?> GetValidTokenAsync(int userId, string code);
    Task<PasswordResetToken> AddAsync(PasswordResetToken token);
    Task UpdateAsync(PasswordResetToken token);
    Task InvalidateUserTokensAsync(int userId);
}
