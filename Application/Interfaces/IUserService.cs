using Application.DTOs.User;
namespace Application.Interfaces;

public interface IUserService
{
    Task <UserDto> CreateMasterAsync(CreateMasterDto createMasterDto,int companyId );
}