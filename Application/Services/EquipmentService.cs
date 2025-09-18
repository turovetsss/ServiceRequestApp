using Application.DTOs.Equipment;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using IEquipmentPhotoRepository = Application.Interfaces.IEquipmentPhotoRepository;

namespace Application.Services;

public class EquipmentService(IEquipmentRepository equipmentRepository, IEquipmentPhotoRepository photoRepository)
    : IEquipmentService
{
    public async Task<EquipmentDto> GetEquipmentByIdAsync(int id)
    {
        var equipment = await equipmentRepository.GetEquipmentByIdAsync(id);
        if (equipment == null) throw new Exception("Equipment not found");
        return new EquipmentDto
        {
            Id = equipment.Id,
            Name = equipment.Name,
            Description = equipment.Description,
            CompanyId = equipment.CompanyId
        };
    }

    public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync()
    {
        var equipments = await equipmentRepository.GetAllAsync();
        return equipments.Where(e => e != null).Select(e => new EquipmentDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description
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

    public async Task<EquipmentDto> UpdateEquipmentAsync(int id, UpdateEquipmentDto updateDto)
    {
        var equipment = await equipmentRepository.GetEquipmentByIdAsync(id);
        if (equipment == null) throw new Exception("Equipment not found");
        equipment.Name = updateDto.Name;
        equipment.Description = updateDto.Description;
        await equipmentRepository.UpdateEquipmentAsync(equipment);
        return await GetEquipmentByIdAsync(id);
    }

    public async Task DeleteEquipmentAsync(int id)
    {
        var equipment = await equipmentRepository.GetEquipmentByIdAsync(id);
        if (equipment == null) throw new Exception("Equipment not found");
        await equipmentRepository.DeleteEquipmentAsync(id);
    }

    public async Task AddEquipmentPhotoAsync(int equipmentId, string photoUrl, string objectKey)
    {
        var equipment = await equipmentRepository.GetEquipmentByIdAsync(equipmentId);
        if (equipment == null) throw new Exception("Equipment not found");
        var photo = new EquipmentPhotoDto
        {
            EquipmentId = equipmentId,
            PhotoUrl = photoUrl,
            ObjectKey = objectKey
        };
        await photoRepository.AddAsync(photo);
        await photoRepository.SaveChangesAsync();
    }


    public async Task RemoveEquipmentPhotoAsync(int photoId)
    {
        var photo = await photoRepository.GetByIdAsync(photoId);
        if (photo == null) throw new Exception("Photo not found");

        var photoDto = new EquipmentPhotoDto
        {
            Id = photo.Id,
            EquipmentId = photo.EquipmentId,
            PhotoUrl = photo.PhotoUrl,
            ObjectKey = photo.ObjectKey
        };

        await photoRepository.DeleteAsync(photoDto);
        await photoRepository.SaveChangesAsync();
    }
}