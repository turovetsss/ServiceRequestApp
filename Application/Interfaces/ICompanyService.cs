using Application.DTOs.Company;
using Domain.Entities;
using Domain.Enums;
namespace Application.Interfaces;

public interface ICompanyService
{
    Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto);
    Task<CompanyDto> GetCompanyByIdAsync(int companyId);
    Task<CompanyDto> UpdateCompanyAsync(int companyId,UpdateCompanyDto updateCompanyDto);
    Task DeleteCompanyAsync(int requestId);
    Task<IEnumerable<CompanyDto>> GetAllAsync();
}