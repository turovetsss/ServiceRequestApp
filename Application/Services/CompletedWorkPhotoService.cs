using Application.DTOs.Request;
using Application.Interfaces;
using Domain.Entities;
namespace Application.Services;

public class CompletedWorkPhotoService:ICompletedWorkPhotoRepository
{
    private readonly Domain.Interfaces.ICompletedWorkPhotoRepository _repository;

    public CompletedWorkPhotoService(Domain.Interfaces.ICompletedWorkPhotoRepository repository)
    {
        _repository = repository;
    }
    public async Task<CompletedWorkPhoto?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(CompletedWorkPhoto photo)
    {
        await _repository.AddAsync(photo);
    }

    public async Task DeleteAsync(CompletedWorkPhoto photo)
    {
        await _repository.DeleteAsync(photo);
    }

    public async Task SaveChangesAsync()
    {
        await _repository.SaveChangesAsync();
    }
}