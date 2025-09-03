using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using UserRole = Domain.Entities.UserRole;

namespace Application.Services;

public class UserService(IUserRepository userRepository):IUserService
{
    public async Task<UserDto> CreateMasterAsync(CreateMasterDto createMasterDto, int companyId)
    {
        var master = new User
        {
            Email = createMasterDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createMasterDto.Password),
            Role = UserRole.Master,
            CompanyId = companyId
        };
        await userRepository.CreateUserAsync(master);
        return new UserDto
        {
            Id = master.Id,
            Email = master.Email,
            Role = master.Role,
            CompanyId = companyId
        };
    }
}

