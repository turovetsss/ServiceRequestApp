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
    [HttpPost("masters")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> CreateMaster(CreateMasterDto createMasterDto)
    {
        var companyId=int.Parse(User.FindFirst("CompanyId")?.Value);
        var master = await userService.CreateMasterAsync(createMasterDto, companyId);
        return Ok(master);
    }

}