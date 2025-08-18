using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public class CompanyRepository(ApplicationDbContext context) : ICompanyRepository
{

    public async Task<Company?> GetCompanyWithDetailsAsync(int id)
    {
        return await context.Companies
            .Include(c=>c.Name)
                .FirstOrDefaultAsync(e => e.Id == id);
       
    }
    public async Task<Company?> GetCompanyByIdAsync(int id)
    {
        return await context.Companies.FindAsync(id);
    }

    public async Task<IEnumerable<Company?>> GetAllAsync()
    {
        return await context.Companies.ToListAsync();
    }
    
    public async Task CreateCompanyAsync(Company? company)
    {
        context.Companies.Add(company);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCompanyAsync(Company? company)
    {
        context.Companies.Update(company);
        await context.SaveChangesAsync();
    }

    public async Task DeleteCompanyAsync(int id)
    {
        var company = await GetCompanyWithDetailsAsync(id);
        if (company != null)
        {
            context.Companies.Remove(company);
            await context.SaveChangesAsync();
        }

    }
}
