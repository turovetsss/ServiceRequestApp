using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles="Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService):ControllerBase
{
    [HttpPost("post-masters")]
    public async Task<ActionResult<IEnumerable<UserDto>>> CreateMaster(CreateMasterDto createMasterDto)
    {
        var companyId=int.Parse(User.FindFirst("CompanyId")?.Value);
        var master = await userService.CreateMasterAsync(createMasterDto, companyId);
        return Ok(master);
    }

}