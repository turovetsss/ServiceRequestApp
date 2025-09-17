using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CompletedWorkPhotoRepository : ICompletedWorkPhotoRepository
{
    private readonly ApplicationDbContext db;

    public CompletedWorkPhotoRepository(ApplicationDbContext db)
    {
        this.db = db;
    }

    public Task<CompletedWorkPhoto?> GetByIdAsync(int id)
    {
        return db.CompletedWorkPhotos.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(CompletedWorkPhoto photo)
    {
        await db.CompletedWorkPhotos.AddAsync(photo);
    }

    public Task DeleteAsync(CompletedWorkPhoto photo)
    {
        db.CompletedWorkPhotos.Remove(photo);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
    {
        return db.SaveChangesAsync();
    }
}