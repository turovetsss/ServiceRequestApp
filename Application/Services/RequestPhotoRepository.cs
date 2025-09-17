using Application.DTOs.Request;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class RequestPhotoRepository : IRequestPhotoRepository
{
    private readonly Domain.Interfaces.IRequestPhotoRepository _repository;

    public RequestPhotoRepository(Domain.Interfaces.IRequestPhotoRepository repository)
    {
        _repository = repository;
    }

    public async Task<RequestPhoto?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(RequestPhotoDto photo)
    {
        var entity = new RequestPhoto
        {
            RequestId = photo.RequestId,
            PhotoUrl = photo.PhotoUrl,
            ObjectKey = photo.ObjectKey
        };
        await _repository.AddAsync(entity);
    }

    public async Task DeleteAsync(RequestPhotoDto photo)
    {
        var entity = await _repository.GetByIdAsync(photo.Id);
        if (entity != null) await _repository.DeleteAsync(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _repository.SaveChangesAsync();
    }
}