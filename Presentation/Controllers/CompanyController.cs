using Application.DTOs.Company;
using Application.Interfaces;
using Application.Services;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController(ICompanyService companyService):ControllerBase
{
  
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCompany(int id)
    {
        var company = await companyService.GetCompanyByIdAsync(id);
        if (company == null)
            return NotFound();
        return Ok(company);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyDto updateDto)
    {
        var company= await companyService.UpdateCompanyAsync(id, updateDto);
        return Ok(company);
    }

}