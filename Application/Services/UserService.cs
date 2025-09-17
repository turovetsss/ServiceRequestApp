using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using UserRole = Domain.Entities.UserRole;

namespace Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserDto> CreateMasterAsync(CreateMasterDto createMasterDto, int companyId)
    {
        var master = new User
        {
            Email = createMasterDto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(createMasterDto.Password),
            Role = UserRole.Master,
            CompanyId = companyId,
            IsActive = true
        };
        await userRepository.CreateUserAsync(master);
        return new UserDto
        {
            Id = master.Id,
            Email = master.Email,
            Role = master.Role,
            CompanyId = companyId,
            IsActive = true
        };
    }

    public async Task<PagedMastersDto> GetMastersByCompanyAsync(int companyId, int page, int size,
        bool? isActive = null)
    {
        var masters = await userRepository.GetMastersByCompanyIdAsync(companyId, page, size, isActive);
        var totalCount = await userRepository.GetMasterCountAsyncByCompanyId(companyId, isActive);

        return new PagedMastersDto
        {
            Users = masters.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Role = u.Role,
                CompanyId = u.CompanyId,
                IsActive = u.IsActive
            }).ToList(),
            TotalCount = totalCount,
            PageSize = size,
            PageNumber = page
        };
    }

    public async Task<UserDto> GetMasterByIdAsync(int id, int companyId)
    {
        var master = await userRepository.GetMastersByIdAsync(id, companyId);
        if (master == null) throw new Exception("Мастер не найден");

        return new UserDto
        {
            Id = master.Id,
            Email = master.Email,
            Role = master.Role,
            CompanyId = master.CompanyId,
            IsActive = master.IsActive
        };
    }

    public async Task<UserDto> UpdateMasterAsync(int id, UpdateMasterDto updateMasterDto, int companyId)
    {
        var master = await userRepository.GetMastersByIdAsync(id, companyId);
        if (master == null) throw new Exception("Мастер не найден");
        master.Email = updateMasterDto.Email;
        master.IsActive = updateMasterDto.isActive;

        await userRepository.UpdateUserAsync(master);
        return new UserDto
        {
            Id = master.Id,
            Email = master.Email,
            Role = master.Role,
            CompanyId = master.CompanyId,
            IsActive = master.IsActive
        };
    }

    public async Task DeactivateMasterAsync(int id, int companyId)
    {
        var master = userRepository.GetMastersByIdAsync(id, companyId);
        if (master == null) throw new Exception("Мастер не найден");
        //master.IsActive=false;
        //await userRepository.UpdateUserAsync(master);
    }

    public async Task<UserDto> ActivateMasterAsync(int id, int companyId)
    {
        throw new NotImplementedException();
    }
}