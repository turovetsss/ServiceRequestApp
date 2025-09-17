using Application.DTOs.Company;

namespace Application.Interfaces;

public interface ICompanyService
{
    Task<CompanyDto> GetCompanyByIdAsync(int companyId);
    Task<CompanyDto> UpdateCompanyAsync(int companyId, UpdateCompanyDto updateCompanyDto);
    Task<IEnumerable<CompanyDto>> GetAllAsync();
}