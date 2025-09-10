using Application.DTOs.Equipment;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class EquipmentPhotoRepository : IEquipmentPhotoRepository
{
    private readonly Domain.Interfaces.IEquipmentPhotoRepository _domainRepository;

    public EquipmentPhotoRepository(Domain.Interfaces.IEquipmentPhotoRepository domainRepository)
    {
        _domainRepository = domainRepository;
    }

    public async Task<EquipmentPhoto?> GetByIdAsync(int id)
    {
        return await _domainRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(EquipmentPhotoDto photo)
    {
        var entity = new EquipmentPhoto
        {
            EquipmentId = photo.EquipmentId,
            PhotoUrl = photo.PhotoUrl,
            ObjectKey = photo.ObjectKey
        };
        await _domainRepository.AddAsync(entity);
    }

    public async Task DeleteAsync(EquipmentPhotoDto photo)
    {
        var entity = await _domainRepository.GetByIdAsync(photo.Id);
        if (entity != null)
        {
            await _domainRepository.DeleteAsync(entity);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _domainRepository.SaveChangesAsync();
    }
}