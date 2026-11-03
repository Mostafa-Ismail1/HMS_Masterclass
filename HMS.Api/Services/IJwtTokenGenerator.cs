namespace HMS.Api.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(string username, string role, string? department);
}
