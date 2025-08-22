using Application.DTOs.User;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using UserRole = Domain.Entities.UserRole;

namespace Application.Services;

public class AuthService(IUserRepository userRepository, ICompanyRepository companyRepository,IConfiguration configuration): IAuthService
{

    public async Task<AuthResponseDto> RegisterAdminAsync(RegisterDto registerDto)
    {
        var company = new Company
        {
            Name = registerDto.CompanyName,
        };
        await companyRepository.CreateCompanyAsync(company);
        string passwordHash=BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        var admin = new User
        {
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            Role = UserRole.Admin,
            CompanyId = company.Id,
        };
        await userRepository.CreateUserAsync(admin);
        return GenerateJwtResponse(admin);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user=await userRepository.GetUserByEmailAsync(loginDto.Email);
        if (user == null|| !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Неверные логин или пароль");
        }
        return GenerateJwtResponse(user);
    }

    private AuthResponseDto GenerateJwtResponse(User user)
    {
        string secretKey=configuration["JWT:Secret"];
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("CompanyId", user.CompanyId.ToString()),
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