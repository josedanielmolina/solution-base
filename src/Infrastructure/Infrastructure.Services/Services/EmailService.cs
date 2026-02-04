using Core.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // TODO: Implement actual email sending logic
        _logger.LogInformation("Email sent to {To} with subject: {Subject}", to, subject);
        await Task.CompletedTask;
    }

    public async Task SendVerificationEmailAsync(string to, string verificationLink)
    {
        var subject = "Verify your email address";
        var body = $"Please click the following link to verify your email: {verificationLink}";
        await SendEmailAsync(to, subject, body);
    }

    public async Task SendPasswordResetEmailAsync(string to, string resetLink)
    {
        var subject = "Reset your password";
        var body = $"Please click the following link to reset your password: {resetLink}";
        await SendEmailAsync(to, subject, body);
    }

    public async Task SendPasswordResetCodeAsync(string to, string userName, string code)
    {
        var subject = "Your password reset code";
        var body = $@"Hello {userName},

Your password reset code is: {code}

This code will expire in 5 minutes.

If you did not request a password reset, please ignore this email.";
        await SendEmailAsync(to, subject, body);
    }
}

