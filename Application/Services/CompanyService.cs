using Application.DTOs.Company;
using Application.Interfaces;
using Application.DTOs.Company;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
namespace Application.Services;

public class CompanyService(ICompanyRepository companyRepository): ICompanyService
{
    private readonly ICompanyRepository _companyRepository = companyRepository;

    public async Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto)
    {
        var company = new Company
        {
            Name = createCompanyDto.Name
        };
        await companyRepository.CreateCompanyAsync(company);
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name
        };
    }

    public async Task<CompanyDto> GetCompanyByIdAsync(int companyId)
    {
        var company =  await companyRepository.GetCompanyByIdAsync(companyId);
        if (company == null) throw new Exception("Company not found");
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name
        };
    }

    public async Task<CompanyDto> UpdateCompanyAsync(int companyId,UpdateCompanyDto updateCompanyDto)
    {
        var company=await companyRepository.GetCompanyByIdAsync(companyId);
        if(updateCompanyDto.Name!=null) 
            company.Name = updateCompanyDto.Name;
        await companyRepository.UpdateCompanyAsync(company);
        return await GetCompanyByIdAsync(companyId);
    }

    public async Task DeleteCompanyAsync(int companyId)
    {
        var company =  companyRepository.GetCompanyByIdAsync(companyId);
        await companyRepository.DeleteCompanyAsync(companyId);
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync()
    {
        var companies = await companyRepository.GetAllAsync();
        return companies.Where(c=>c!=null).Select(c=>new CompanyDto
            {Id=c.Id,Name=c.Name}).ToList();
    }
}