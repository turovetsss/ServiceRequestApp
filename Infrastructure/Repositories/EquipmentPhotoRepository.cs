using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EquipmentPhotoRepository : IEquipmentPhotoRepository
    {
        private readonly ApplicationDbContext db;

        public EquipmentPhotoRepository(ApplicationDbContext db) => this.db = db;

        public Task<EquipmentPhoto?> GetByIdAsync(int id) => db.EquipmentPhotos.FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(EquipmentPhoto photo)
        {
            await db.EquipmentPhotos.AddAsync(photo);
        }

        public Task DeleteAsync(EquipmentPhoto photo)
        {
            db.EquipmentPhotos.Remove(photo);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => db.SaveChangesAsync();
    }
}