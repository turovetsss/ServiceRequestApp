using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context):IUserRepository
{
    public async Task<User?> GetUserWithDetailsAsync(int id)
    {
        return await context.Users
            .Include(u=>u.Company)
            .Include(u=>u.CreatedRequests)
            .Include(u=>u.AssignedRequests)
            .Include(u=>u.StatusChanges)
            .FirstOrDefaultAsync(u=>u.Id == id);
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User?>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task CreateUserAsync(User? user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User? user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await GetUserWithDetailsAsync(id);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
        
    }
    
}