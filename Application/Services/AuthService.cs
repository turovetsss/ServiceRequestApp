using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserRole = Domain.Entities.UserRole;

namespace Application.Services;

public class AuthService(
    IUserRepository userRepository,
    ICompanyRepository companyRepository,
    IConfiguration configuration) : IAuthService
{
    public async Task<AuthResponseDto> RegisterAdminAsync(RegisterDto registerDto)
    {
        var company = new Company
        {
            Name = registerDto.CompanyName
        };
        await companyRepository.CreateCompanyAsync(company);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        var admin = new User
        {
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            Role = UserRole.Admin,
            CompanyId = company.Id
        };
        await userRepository.CreateUserAsync(admin);
        return GenerateJwtResponse(admin);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Неверные логин или пароль");

        return GenerateJwtResponse(user);
    }

    private AuthResponseDto GenerateJwtResponse(User user)
    {
        var secretKey = configuration["Jwt:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("CompanyId", user.CompanyId.ToString())
        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return new AuthResponseDto
        {
            Token = tokenString,
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role,
            CompanyId = user.CompanyId
        };
    }
}