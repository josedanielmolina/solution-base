using Core.Application.Common.Result;
using Core.Application.DTOs.Auth;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;

namespace Core.Application.Features.Auth;

public class RequestPasswordReset
{
    private const int CodeExpirationMinutes = 5;
    
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _tokenRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public RequestPasswordReset(
        IUserRepository userRepository,
        IPasswordResetTokenRepository tokenRepository,
        IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> ExecuteAsync(RequestPasswordResetDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        
        // Always return success to prevent email enumeration
        if (user == null || !user.IsActive)
        {
            return Result.Success();
        }

        // Invalidate any existing tokens
        await _tokenRepository.InvalidateUserTokensAsync(user.Id);

        // Generate 6-digit code
        var code = GenerateCode();

        var token = new PasswordResetToken
        {
            UserId = user.Id,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(CodeExpirationMinutes),
            IsUsed = false
        };

        await _tokenRepository.AddAsync(token);
        await _unitOfWork.SaveChangesAsync();

        // Send email
        await _emailService.SendPasswordResetCodeAsync(user.Email, user.FirstName, code);

        return Result.Success();
    }

    private static string GenerateCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}

