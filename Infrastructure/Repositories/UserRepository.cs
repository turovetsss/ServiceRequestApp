using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context):IUserRepository
{
   
    public async Task<User?> GetByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User?>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task CreateUserAsync(User? user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }
    

    public async Task<List<User>> GetMastersByCompanyIdAsync(int companyId, int page, int size, bool? isActive = null)
    {
        var masters = context.Users
            .Where(u => u.CompanyId == companyId && u.Role == UserRole.Master);
        return await masters
            .OrderBy(u => u.Email)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<User?> GetMastersByIdAsync(int id, int companyId)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.CompanyId == companyId && u.Role == UserRole.Master);
    }

    public async Task UpdateUserAsync(User? user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetMasterCountAsyncByCompanyId(int companyId, bool? isActive = null)
    {
        var count = context.Users
            .Where(u => u.CompanyId == companyId && u.Role == UserRole.Master);
        if (isActive != null)
        {
            count = count.Where(u => u.IsActive == isActive.Value);
        }
        return await count.CountAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
        
    }
    
}