using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService):ControllerBase
{
    [HttpPost("register-admin")]
    public async Task<ActionResult<AuthResponseDto>> RegisterAdmin(RegisterDto registerDto)
    {
        var response = await authService.RegisterAdminAsync(registerDto);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var response = await authService.LoginAsync(loginDto);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }  catch (Exception ex)
        {
            return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
        }
    }
}