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
    [HttpPost]
    [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyDto createDto)
    {
        var company = await companyService.CreateCompanyAsync(createDto);
        return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
    }

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
}