using Application.DTOs.Equipment;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class EquipmentService(IEquipmentRepository equipmentRepository):IEquipmentService
{
    public async Task<EquipmentDto> GetEquipmentByIdAsync(int id)
    {
        var equipment = await equipmentRepository.GetEquipmentByIdAsync(id);
        if(equipment == null)throw new Exception("Equipment not found");
        return new EquipmentDto
        {
            Id = equipment.Id,
            Name = equipment.Name,
            Description = equipment.Description,
            CompanyId=equipment.CompanyId
        };
    }

    public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync()
    {
        var equipments = await equipmentRepository.GetAllAsync();
        return equipments.Where(e=>e!=null).Select(e=>new EquipmentDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
        }).ToList();
    
}

    public async Task<EquipmentDto> CreateEquipmentAsync(CreateEquipmentDto createDto, int companyId)
    {
        var equipment = new Equipment
        {
            Name = createDto.Name,
            Description = createDto.Description,
            CompanyId = companyId
        };
        await equipmentRepository.CreateEquipmentAsync(equipment);
        return new EquipmentDto
        {
            Id = equipment.Id,
            Name = equipment.Name,
            Description = equipment.Description,
            CompanyId = equipment.CompanyId
        };
    }
    
    public async Task<EquipmentDto> UpdateEquipmentAsync(int id,UpdateEquipmentDto updateDto)
    {
        var equipment=await equipmentRepository.GetEquipmentByIdAsync(id);
        if (equipment == null)throw new Exception("Equipment not found");
        equipment.Name = updateDto.Name;
        equipment.Description = updateDto.Description;
        await equipmentRepository.UpdateEquipmentAsync(equipment);
        return await GetEquipmentByIdAsync(id);
    }

    public async Task DeleteEquipmentAsync(int id)
    {
        var equipment=await equipmentRepository.GetEquipmentByIdAsync(id);
        await equipmentRepository.DeleteEquipmentAsync(id);
    }

    public Task AddEquipmentPhotoAsync(int equipmentId, string photoUrl)
    {
        throw new NotImplementedException();
    }

    public Task RemoveEquipmentPhotoAsync(int photoId)
    {
        throw new NotImplementedException();
    }
}