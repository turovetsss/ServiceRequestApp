using Application.DTOs.User;
namespace Application.Interfaces;

public interface IUserService
{
    Task <UserDto> CreateMasterAsync(CreateMasterDto createMasterDto,int companyId );
    Task<PagedMastersDto> GetMastersByCompanyAsync(int companyId,int page,int size,bool? isActive=null);
    Task<UserDto> GetMasterByIdAsync(int id,int companyId);
    Task<UserDto> UpdateMasterAsync(int id,UpdateMasterDto updateMasterDto,int companyId);
    Task DeactivateMasterAsync(int id,int companyId);
    Task<UserDto> ActivateMasterAsync(int id,int companyId);

}