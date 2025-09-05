using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User?>> GetAllAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task CreateUserAsync(User? user);
    Task<List<User>> GetMastersByCompanyIdAsync(int companyId,int page,int size, bool? isActive = null);
    Task<User?> GetMastersByIdAsync(int id,int companyId);
    Task UpdateUserAsync(User? user);
    Task<int> GetMasterCountAsyncByCompanyId(int companyId,bool? isActive = null);
}