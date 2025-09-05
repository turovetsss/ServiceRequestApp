using System.Data;
using System.Security.Claims;
using Application.Dto.Request;
using Application.DTOs.Request;
using Application.Interfaces;
using Application.Services;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RequestStatus = Domain.Entities.RequestStatus;

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
        var companyId = int.Parse(User.FindFirst("CompanyId").Value);
        var adminId = int.Parse(User.FindFirst("UserId").Value);
        
        var request = await requestService.CreateRequestAsync(createDto, companyId, adminId);
        return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRequests([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] RequestStatus? status = null)
    {
        var companyId = int.Parse(User.FindFirst("CompanyId").Value);
        var ifisMaster = User.IsInRole("Master");
        var userId = int.Parse(User.FindFirst("UserId").Value);
        List<RequestDto> requests = null;
        if (ifisMaster)
        {
            var allRequests=await requestService.GetAllRequestsByCompanyIdAsync(companyId, page, pageSize,status); 
            requests = allRequests.Where(r=>r.AssignedMasterId==userId).ToList();
        }
        else
        {
            requests = await requestService.GetAllRequestsByCompanyIdAsync(companyId, page, pageSize, status);
        }
        return Ok(requests);
    }

    [HttpPatch("{id}/assign-master")]
    public async Task<IActionResult> AssignMaster(int id, [FromBody] AssignMasterDto assignMasterDto)
    {
        await requestService.AssignMasterAsync(id,assignMasterDto.UserId);
        return Ok();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto updateStatusDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        await requestService.UpdateStatusAsync(id, updateStatusDto.NewStatus, userId);
        return Ok();
    }

    [HttpGet("{id}/status-history")]
    public async Task<IActionResult> GetStatusHistory(int id)
    {
        var history=await requestService.GetStatusHistoryAsync(id);
        return Ok(history);
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