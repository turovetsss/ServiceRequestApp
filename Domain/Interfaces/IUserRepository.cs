using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserWithDetailsAsync(int id);
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User?>> GetAllAsync();
    Task CreateUserAsync(User? user);
    Task UpdateUserAsync(User? user);
    Task DeleteUserAsync(int id);
}