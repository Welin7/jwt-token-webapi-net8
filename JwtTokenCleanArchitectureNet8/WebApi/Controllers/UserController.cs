using Application.Contracts;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUser _user;

    public UserController(IUser user)
    {
        _user = user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> LoginUser(LoginDTO loginDTO)
    {
        var result = await _user.LoginUserAsync(loginDTO);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponse>> RegisterUser(RegisterUserDTO registerUerDTO)
    {
        var result = await _user.RegisterUserAsync(registerUerDTO);
        return Ok(result);
    }
}
