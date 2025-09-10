using System.Data;
using System.Security.Claims;
using Application.Dto.Request;
using Application.DTOs.Request;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using RequestStatus = Domain.Entities.RequestStatus;

namespace Presentation.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RequestsController(IRequestService requestService, IFileStorageService fileStorageService, Application.Interfaces.ICompletedWorkPhotoRepository completedWorkPhotoRepository) : ControllerBase
{
    private const string CompletedWorkBucket= "completed-work-photos";
    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RequestDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDto createDto)
    {
        var companyId = int.Parse(User.FindFirst("CompanyId").Value);
        var adminId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        var request = await requestService.CreateRequestAsync(createDto, companyId, adminId);
        return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, request);
    }
    [HttpGet("Master/assigned-to-me")]
    [Authorize(Roles = "Master")]
    public async Task<ActionResult<List<Request>>> GetAssignedRequests(
        [FromQuery] RequestStatus? status, 
        [FromQuery] int page = 1, 
        [FromQuery] int size = 20)
    {
        try
        {
            var masterId = GetCurrentUserId();
            var requests = await requestService.GetAssignedRequestsAsync(masterId, status, page, size);
            return Ok(requests);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpPatch("{id}/master/accept")]
    [Authorize(Roles = "Master")]
    public async Task<ActionResult<Request>> AcceptRequest(int id)
    {
        try
        {
            var masterId = GetCurrentUserId();
            var request = await
                requestService.MasterAcceptRequestAsync(id, masterId);
            return Ok(request);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    [HttpPatch("{id}/master/start-work")]
    [Authorize(Roles = "Master")]
    public async Task<ActionResult<Request>> StartWork(int id)
    {
        try
        {
            var masterId = GetCurrentUserId();
            var request = await requestService.MasterStartWorkAsync(id, masterId);
            return Ok(request);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/complete-work")]
    [Authorize(Roles = "Master")]
    public async Task<ActionResult<Request>> CompleteWork(int id, [FromBody] CompletedWorkDto dto)
    {
        try
        {
            var masterId = GetCurrentUserId();
            var request = await requestService.MasterCompletedWorkAsync(id, masterId, dto.CompletedWorkPhotoUrls);
            return Ok(request);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    
    }

    [HttpPost("{id}/master/upload-completed-photo")]
    [Authorize(Roles = "Master")]
    public async Task<ActionResult<CompletedWorkPhotoDto>> UploadCompletedPhoto(int id, [FromForm] IFormFile file)
    {
        try
        {
            var masterId = GetCurrentUserId();
            var request = await requestService.GetRequestByIdAsync(id);
            if (request == null)
                return NotFound($"Request with id {id} not found");

            if (request.AssignedMasterId != masterId)
                return Forbid("Only assigned master can upload completed work photos");

            if (request.Status != "InProgress")
                return BadRequest("Only requests with 'InProgress' status can have completed work photos uploaded");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using var stream = file.OpenReadStream();

            var photoUrl = await fileStorageService.UploadAsync(
                stream,
                file.ContentType,
                "completed-work-photos",
                fileName);

            var completedWorkPhoto = new CompletedWorkPhoto
            {
                RequestId = id,
                PhotoUrl = photoUrl,
                ObjectKey = fileName
            };

            await completedWorkPhotoRepository.AddAsync(completedWorkPhoto);
            await completedWorkPhotoRepository.SaveChangesAsync();

            var photoDto = new CompletedWorkPhotoDto
            {
                Id = completedWorkPhoto.Id,
                RequestId = completedWorkPhoto.RequestId,
                PhotoUrl = completedWorkPhoto.PhotoUrl,
                ObjectKey = completedWorkPhoto.ObjectKey,
                CreatedAt = completedWorkPhoto.CreatedAt
            };

            return Ok(photoDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/completed-work-photos")]
    [Authorize(Roles = "Master")]
    [ProducesResponseType(typeof(List<CompletedWorkPhotoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadCompletedWorkPhotos(int id, [FromForm] UploadCompletedWorkPhotosDto dto)
    {
        try
        {
            if (dto.Photos == null || dto.Photos.Length == 0)
            {
                return BadRequest("No photos provided");
            }

            var masterId = GetCurrentUserId();
            var request = await requestService.GetRequestByIdAsync(id);
            if (request == null)
            {
                return NotFound($"Request with ID {id} not found");
            }

            if (request.AssignedMasterId != masterId)
            {
                return Forbid("Only assigned master can upload completed work photos");
            }

            if (request.Status != "InProgress")
            {
                return BadRequest("Only requests with 'InProgress' status can have completed work photos uploaded");
            }

            var uploadedPhotos = new List<CompletedWorkPhotoDto>();

            foreach (var file in dto.Photos)
            {
                if (file.Length == 0)
                    continue;

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                using var stream = file.OpenReadStream();

                var photoUrl = await fileStorageService.UploadAsync(
                    stream,
                    file.ContentType,
                    "completed-work-photos",
                    fileName);

                var completedWorkPhoto = new CompletedWorkPhoto
                {
                    RequestId = id,
                    PhotoUrl = photoUrl,
                    ObjectKey = fileName
                };

                await completedWorkPhotoRepository.AddAsync(completedWorkPhoto);
                await completedWorkPhotoRepository.SaveChangesAsync();

                uploadedPhotos.Add(new CompletedWorkPhotoDto
                {
                    Id = completedWorkPhoto.Id,
                    RequestId = completedWorkPhoto.RequestId,
                    PhotoUrl = completedWorkPhoto.PhotoUrl,
                    ObjectKey = completedWorkPhoto.ObjectKey,
                    CreatedAt = completedWorkPhoto.CreatedAt
                });
            }

            return Ok(uploadedPhotos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Master")]
    [ProducesResponseType(typeof(List<RequestDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRequests([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] RequestStatus? status = null)
    {
        var companyId = int.Parse(User.FindFirst("CompanyId").Value);
        var ifisMaster = User.IsInRole("Master");
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
    [Authorize(Roles = "Admin")]
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