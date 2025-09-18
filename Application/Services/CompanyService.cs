using Application.DTOs.Company;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public class CompanyService(ICompanyRepository companyRepository) : ICompanyService
{
    public async Task<CompanyDto> GetCompanyByIdAsync(int id)
    {
        var company = await companyRepository.GetCompanyByIdAsync(id);
        if (company == null) throw new Exception("Company not found");
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name
        };
    }

    public async Task<CompanyDto> UpdateCompanyAsync(int companyId, UpdateCompanyDto updateCompanyDto)
    {
        var company = await companyRepository.GetCompanyByIdAsync(companyId);
        if (updateCompanyDto.Name != null)
        {
            company.Name = updateCompanyDto.Name;
        }

        await companyRepository.UpdateCompanyAsync(company);
        return await GetCompanyByIdAsync(companyId);
    }
}