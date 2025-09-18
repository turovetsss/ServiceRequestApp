using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Filters;

public class FileUploadOperationFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var fileParams = context.MethodInfo.GetParameters()
			.Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IFormFile[]))
			.ToList();

		if (fileParams.Any())
		{
			operation.RequestBody = new OpenApiRequestBody
			{
				Content = new Dictionary<string, OpenApiMediaType>
				{
					["multipart/form-data"] = new()
					{
						Schema = new OpenApiSchema
						{
							Type = "object",
							Properties = fileParams.ToDictionary(
								p => p.Name!,
								p => new OpenApiSchema
								{
									Type = p.ParameterType == typeof(IFormFile[]) ? "array" : "string",
									Format = "binary",
									Items = p.ParameterType == typeof(IFormFile[])
										? new OpenApiSchema { Type = "string", Format = "binary" }
										: null
								}
							),
							Required = fileParams.Where(p => !p.HasDefaultValue).Select(p => p.Name!).ToHashSet()
						}
					}
				}
			};
		}
	}
}
