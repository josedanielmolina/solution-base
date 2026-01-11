namespace Core.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string email, string firstName, string lastName);
}
