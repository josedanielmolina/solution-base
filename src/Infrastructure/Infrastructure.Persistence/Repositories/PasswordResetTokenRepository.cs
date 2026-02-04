using Core.Domain.Entities;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<PasswordResetToken> _dbSet;

    public PasswordResetTokenRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<PasswordResetToken>();
    }

    public async Task<PasswordResetToken?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<PasswordResetToken?> GetValidTokenAsync(int userId, string code)
    {
        return await _dbSet
            .Where(t => t.UserId == userId 
                && t.Code == code 
                && !t.IsUsed 
                && t.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();
    }

    public async Task<PasswordResetToken> AddAsync(PasswordResetToken token)
    {
        token.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(token);
        return token;
    }

    public Task UpdateAsync(PasswordResetToken token)
    {
        _context.Entry(token).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task InvalidateUserTokensAsync(int userId)
    {
        var tokens = await _dbSet
            .Where(t => t.UserId == userId && !t.IsUsed)
            .ToListAsync();

        foreach (var token in tokens)
        {
            token.IsUsed = true;
        }
    }
}
