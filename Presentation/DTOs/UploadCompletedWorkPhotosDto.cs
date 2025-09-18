namespace Presentation.DTOs;

public class UploadCompletedWorkPhotosDto
{
	public IFormFile[] Photos { get; set; } = Array.Empty<IFormFile>();
}
