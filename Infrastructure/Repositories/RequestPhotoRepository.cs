using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RequestPhotoRepository : IRequestPhotoRepository
{
    private readonly ApplicationDbContext db;

    public RequestPhotoRepository(ApplicationDbContext db)
    {
        this.db = db;
    }

    public Task<RequestPhoto?> GetByIdAsync(int id)
    {
        return db.RequestPhotos.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(RequestPhoto photo)
    {
        await db.RequestPhotos.AddAsync(photo);
    }

    public Task DeleteAsync(RequestPhoto photo)
    {
        db.RequestPhotos.Remove(photo);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return db.SaveChangesAsync();
    }
}