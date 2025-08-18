using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Domain.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetCompanyWithDetailsAsync(int id);
    Task<Company?> GetCompanyByIdAsync(int id);
    Task<IEnumerable<Company?>> GetAllAsync();
    Task CreateCompanyAsync(Company? company);
    Task UpdateCompanyAsync(Company? company);
    Task DeleteCompanyAsync(int id);

}