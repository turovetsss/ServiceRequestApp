using Application.DTOs.User;
namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAdminAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
}