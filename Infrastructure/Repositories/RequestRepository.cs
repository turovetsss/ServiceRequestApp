using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class RequestRepository(ApplicationDbContext context):IRequestRepository
{
    public async Task<Request?> GetRequestWithDetailsAsync(int id)
    {
        return await context.Requests
            .Include(r => r.Company)
            .Include(r => r.Equipment)
            .Include(e => e.ProblemPhotos)
            .Include(r => r.CreatedByAdmin)
            .Include(r => r.AssignedMaster)
            .Include(r => r.CompletedWorkPhotos)
            .Include(r => r.Documents)
            .Include(r => r.StatusHistory)
            .ThenInclude(h => h.ChangedByUser)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == id);
       
    }
    public async Task<Request?> GetRequestByIdAsync(int id)
    {
        return await context.Requests.FindAsync(id);
    }
    
    public async Task<IEnumerable<Request?>> GetAllAsync()
    {
        return await context.Requests.ToListAsync();
    }
    
    public async Task CreateRequestAsync(Request? request)
    {
        context.Requests.Add(request);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateRequestAsync(Request? request)
    {
        context.Requests.Update(request);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteRequstAsync(int id)
    {
        var request = await GetRequestWithDetailsAsync(id);
        if(request != null)
        {
            context.Requests.Remove(request);
            await context.SaveChangesAsync();
        }
        
    }
}
