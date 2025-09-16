using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers;

[ApiController]
[Route("api/completed-work-photos")]
public class CompletedWorkPhotoController:ControllerBase
{

    private const string Bucket = "completed-work-photos";
    private readonly IFileStorageService storage;
    private readonly IRequestService requestService;

    public CompletedWorkPhotoController(IFileStorageService storage, IRequestService requestService)
    {
        this.storage = storage;
        this.requestService = requestService;
    }

    [HttpPost("{requestId:int}/photos")]
    [Authorize(Roles = "Admin,Master")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(int requestId, List<IFormFile> files, CancellationToken ct)
    {
        if (files == null || files.Count == 0) return BadRequest("No files");

        var result = new List<string>();
        foreach (var file in files)
        {
            if (file.Length == 0) continue;
            var ext = Path.GetExtension(file.FileName);
            var key = $"{requestId}/{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid()}{ext}";

            await using var stream = file.OpenReadStream();
            var url = await storage.UploadAsync(stream, file.ContentType, Bucket, key, ct);

            await requestService.AddRequestPhotoAsync(requestId, url, key);
            result.Add(url);
        }

        return Ok(result);
    }

    [HttpDelete("photos/{photoId:int}")]
    [Authorize(Roles = "Admin,Master")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int photoId, CancellationToken ct)
    {
        await requestService.RemoveRequestPhotoAsync(photoId);
        return NoContent();
    }
   
}