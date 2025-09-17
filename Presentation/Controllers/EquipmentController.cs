using Application.DTOs.Equipment;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class EquipmentController(IEquipmentService equipmentService) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EquipmentDto>))]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAllEquipment()
	{
		var equipment = await equipmentService.GetAllEquipmentAsync();
		return Ok(equipment);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetEquipmentById(int id)
	{
		var equipment = await equipmentService.GetEquipmentByIdAsync(id);
		if (equipment == null)
		{
			return NotFound();
		}

		return Ok(equipment);
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	[ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateEquipment([FromBody] CreateEquipmentDto createEquipmentDto)
	{
		var companyId = int.Parse(User.FindFirst("CompanyId").Value);
		var equipment = await equipmentService.CreateEquipmentAsync(createEquipmentDto, companyId);
		return CreatedAtAction(nameof(GetEquipmentById), new { id = equipment.Id }, equipment);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateEquipment(int id, [FromBody] UpdateEquipmentDto updateEquipmentDto)
	{
		var equipment = await equipmentService.UpdateEquipmentAsync(id, updateEquipmentDto);
		return Ok(equipment);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteEquipment(int id)
	{
		await equipmentService.DeleteEquipmentAsync(id);
		return NoContent();
	}
}
