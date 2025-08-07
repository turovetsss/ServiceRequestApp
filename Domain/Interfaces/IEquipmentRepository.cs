using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.Interfaces;

public interface IEquipmentRepository
{
    Task<Equipment?> GetEquipmentWithDetailsAsync(int id);
    Task<Equipment?> GetEquipmentByIdAsync(int id);
    Task<IEnumerable<Equipment?>> GetAllAsync();
    Task CreateEquipmentAsync(Equipment? equipment);
    Task UpdateEquipmentAsync(Equipment? equipment);
    Task DeleteEquipmentAsync(int id);
}