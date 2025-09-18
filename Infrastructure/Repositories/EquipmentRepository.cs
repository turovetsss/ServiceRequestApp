using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EquipmentRepository(ApplicationDbContext context) : IEquipmentRepository
{
    public async Task<Equipment?> GetEquipmentWithDetailsAsync(int id)
    {
        return await context.Equipments
            .Include(e => e.Company)
            .Include(e => e.Photos)
            .Include(e => e.Requests)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(int id)
    {
        return await context.Equipments.FindAsync(id);
    }

    public async Task<IEnumerable<Equipment?>> GetAllAsync()
    {
        return await context.Equipments.ToListAsync();
    }

    public async Task CreateEquipmentAsync(Equipment? equipment)
    {
        context.Equipments.Add(equipment);
        await context.SaveChangesAsync();
    }

    public async Task UpdateEquipmentAsync(Equipment? equipment)
    {
        context.Equipments.Update(equipment);
        await context.SaveChangesAsync();
    }

    public async Task DeleteEquipmentAsync(int id)
    {
        var equipment = await GetEquipmentWithDetailsAsync(id);
        if (equipment != null)
        {
            context.Equipments.Remove(equipment);
            await context.SaveChangesAsync();
        }
    }
}