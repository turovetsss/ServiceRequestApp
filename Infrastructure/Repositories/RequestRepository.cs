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
            .Include(e => e.Photos)
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

    public async Task<List<Request>> GetAssignedToMasterAsync(int masterId, RequestStatus? status, int page, int size)
    {
        var query = context.Requests
            .Where(r => r.AssignedMasterId == masterId)
            .Include(r => r.AssignedMaster)
            .Include(r => r.Company)
            .Include(r => r.Photos)
            .Include(r=>r.StatusHistory)
            .OrderBy(r => r.Id);
        return await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<List<Request>?> GetAllRequestByCompanyIdAsync(int companyId, int page, int size, RequestStatus? status)
    {
        var requests = context.Requests
            .Where(r => r.CompanyId == companyId);

        if (status != null)
        {
            requests = requests.Where(r => r.Status == status.Value);
        }
        return await requests
            .OrderBy(r=>r.Status)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }
}
