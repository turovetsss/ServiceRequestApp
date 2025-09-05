using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserService userService):ControllerBase
{
    [HttpGet("company-id-from-token")]
    public ActionResult<int> GetCompanyIdFromUserToken()
    {
        var companyIdClaim = User.FindFirst("CompanyId");
        if (companyIdClaim == null)
        {
            return Unauthorized("CompanyId claim not found in token");
        }
        
        if (!int.TryParse(companyIdClaim.Value, out int companyId))
        {
            return BadRequest("Invalid CompanyId format in token");
        }
        
        return Ok(companyId);
    }
    [HttpPost("masters")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> CreateMaster(CreateMasterDto createMasterDto)
    {
        var companyIdClaim = User.FindFirst("CompanyId");
        if (companyIdClaim == null || !int.TryParse(companyIdClaim.Value, out int companyId))
        {
            return Unauthorized("CompanyId claim not found or invalid in token");
        }

        var master = await userService.CreateMasterAsync(createMasterDto, companyId);
        return Ok(master);
    }

    [HttpGet("masters")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedMastersDto>> GetMasters([FromQuery] int page = 1, [FromQuery] int size = 10,
        [FromQuery] bool? isActive = null)
    {
        var companyIdClaim = User.FindFirst("CompanyId");
        if (companyIdClaim == null || !int.TryParse(companyIdClaim.Value, out int companyId))
        {
            return Unauthorized("CompanyId claim not found or invalid in token");
        }
        var result=await userService.GetMastersByCompanyAsync(companyId, page, size, isActive);
        return Ok(result);
    }

    [HttpGet("masters/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> GetMasterById(int id)
    {
        var companyIdClaim = User.FindFirst("CompanyId");
        if (companyIdClaim == null || !int.TryParse(companyIdClaim.Value, out int companyId))
        {
            return Unauthorized("CompanyId claim not found or invalid in token");
        }
        var master = await userService.GetMasterByIdAsync(id, companyId);
        return Ok(master);
    }

    [HttpPut("masters/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> UpdateMaster(int id, UpdateMasterDto updateMasterDto)
    {
        var companyIdClaim = User.FindFirst("CompanyId");
        if (companyIdClaim == null || !int.TryParse(companyIdClaim.Value, out int companyId))
        {
            return Unauthorized("CompanyId claim not found or invalid in token");
        }

        var master = await userService.UpdateMasterAsync(id, updateMasterDto, companyId);
        return Ok(master);
    }
}