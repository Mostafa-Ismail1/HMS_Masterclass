using HMS.Api.DTOs.Auth;
using HMS.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IJwtTokenGenerator jwtTokenGenerator) : ControllerBase
{
    private static readonly Dictionary<string, RefreshTokenRecord> _refreshTokens = new();

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        string role;
        string? department = null;

        if (request.Username == "admin" && request.Password == "Admin@123")
        {
            role = "Admin";
        }
        else if (request.Username == "cardiologist" && request.Password == "Cardio@123")
        {
            role = "Doctor";
            department = "Cardiology";
        }
        else if (request.Username == "pediatrician" && request.Password == "Ped@123")
        {
            role = "Doctor";
            department = "Pediatrics";
        }
        else if (request.Username == "doctor" && request.Password == "Doc@123")
        {
            role = "Doctor";
        }
        else
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var accessToken = jwtTokenGenerator.GenerateToken(request.Username, role, department);

        var refreshToken = Guid.NewGuid().ToString("N");
        _refreshTokens[refreshToken] = new RefreshTokenRecord(
            request.Username,
            role,
            department,
            DateTime.UtcNow.AddDays(7)
        );

        return Ok(new AuthResponseDto(accessToken, refreshToken, role, department));
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshTokenRequestDto request)
    {
        if (!_refreshTokens.TryGetValue(request.RefreshToken, out var tokenRecord) || tokenRecord.Expiry < DateTime.UtcNow)
        {
            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }

        var newAccessToken = jwtTokenGenerator.GenerateToken(tokenRecord.Username, tokenRecord.Role, tokenRecord.Department);

        return Ok(new { token = newAccessToken });
    }
}

public record RefreshTokenRecord(string Username, string Role, string? Department, DateTime Expiry);
