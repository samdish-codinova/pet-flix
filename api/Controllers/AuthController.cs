using System.Security.Authentication;
using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{

  [ApiController]
  [Route("/api/[controller]")]
  public class AuthController : Controller
  {
    private IAuthService _authService;
    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }


    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterUserRequestDTO userData)
    {
      User user = await _authService.RegisterUser(userData);

      return Ok(new { id = user.Id, name = user.Name, email = user.Email, createdAt = user.CreatedAt });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseDTO>> Login([FromBody] LoginUserRequestDTO creds)
    {
      LoginUserResponseDTO loginRes = await _authService.Login(creds);

      Response.Headers.Append("x-auth-token", loginRes.AccessToken);
      return Ok(loginRes);
    }
  }
}