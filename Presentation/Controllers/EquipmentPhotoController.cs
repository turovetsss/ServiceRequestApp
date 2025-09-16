using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers;

[ApiController]
[Route("api/equipment-photos")]
public class EquipmentPhotoController: ControllerBase
{
    private const string Bucket = "equipment-photos";
    private readonly IFileStorageService storage;
    private readonly IEquipmentService equipmentService;

    public EquipmentPhotoController(IFileStorageService storage, IEquipmentService equipmentService)
    {
        this.storage = storage;
        this.equipmentService = equipmentService;
    }

    [HttpPost("{equipmentId:int}/photos")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(int equipmentId, List<IFormFile> files, CancellationToken ct)
    {
        if (files == null || files.Count == 0) return BadRequest("No files");

        var result = new List<string>();
        foreach (var file in files)
        {
            if (file.Length == 0) continue;
            var ext = Path.GetExtension(file.FileName);
            var key = $"{equipmentId}/{DateTime.UtcNow:yyyy/MM}/{Guid.NewGuid()}{ext}";

            await using var stream = file.OpenReadStream();
            var url = await storage.UploadAsync(stream, file.ContentType, Bucket, key, ct);

            await equipmentService.AddEquipmentPhotoAsync(equipmentId, url, key);
            result.Add(url);
        }

        return Ok(result);
    }

    [HttpDelete("photos/{photoId:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int photoId, CancellationToken ct)
    {
        await equipmentService.RemoveEquipmentPhotoAsync(photoId);
        return NoContent();
    }
}