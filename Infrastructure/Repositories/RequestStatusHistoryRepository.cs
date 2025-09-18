using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RequestStatusHistoryRepository(ApplicationDbContext context) : IRequestStatusHistoryRepository
{
    public async Task<RequestStatusHistory> GetByIdAsync(int id)
    {
        return await context.RequestStatusHistories
            .Include(h => h.ChangedByUser)
            .FirstOrDefaultAsync(h => h.RequestId == id);
    }

    public async Task<IEnumerable<RequestStatusHistory>> GetByRequestIdAsync(int requestId)
    {
        return await context.RequestStatusHistories
            .Include(h => h.ChangedByUser)
            .Where(h => h.RequestId == requestId)
            .OrderByDescending(h => h.ChangedAt)
            .ToListAsync();
    }

    public async Task AddAsync(RequestStatusHistory entity)
    {
        await context.RequestStatusHistories.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RequestStatusHistory entity)
    {
        context.RequestStatusHistories.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var status = await GetByIdAsync(id);
        if (status != null)
        {
            context.RequestStatusHistories.Remove(status);
            await context.SaveChangesAsync();
        }
    }
}