using Domain.Entities;

namespace Domain.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetCompanyByIdAsync(int id);
    Task<IEnumerable<Company?>> GetAllAsync();
    Task CreateCompanyAsync(Company? company);
    Task UpdateCompanyAsync(Company? company);
    Task DeleteCompanyAsync(int id);
}