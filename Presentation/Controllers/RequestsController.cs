using Application.DTOs.Request;
using Application.Interfaces;
using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController(IRequestService requestService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto createDto)
    {
        var companyId = 1;//int.Parse(User.FindFirst("CompanyId").Value);
        var adminId = 1; //int.Parse(User.FindFirst("UserId").Value);
        
        var request = await requestService.CreateRequestAsync(createDto, companyId, adminId);
        return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRequest(int id)
    {
        var request = await requestService.GetRequestByIdAsync(id);
        if (request == null)
            return NotFound();

        return Ok(request);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRequest(int id, [FromBody] UpdateRequestDto updateDto)
    {
        var request = await requestService.UpdateRequestAsync(id, updateDto);
        return Ok(request);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRequest(int id)
    {
        await requestService.DeleteRequestAsync(id);
        return NoContent();
    }
  
}