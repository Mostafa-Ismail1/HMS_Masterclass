namespace HMS.Api.DTOs.Auth;

public record AuthResponseDto(string Token, string RefreshToken, string Role, string? Department);

public record RefreshTokenRequestDto(string RefreshToken);
