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
    public async Task<ActionResult<RegisterUserResponseDTO>> Register([FromBody] RegisterUserRequestDTO userData)
    {
      User user = await _authService.RegisterUser(userData);
      string accessToken = _authService.GenerateJwt(user);

      Response.Headers.Append("x-access-token", accessToken);

      return new RegisterUserResponseDTO()
      {
        User = new BaseUser()
        {
          Id = user.Id,
          Name = user.Name,
          Email = user.Email,
          Role = user.Role,
          CreatedAt = user.CreatedAt
        },
        AccessToken = accessToken
      };
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseDTO>> Login([FromBody] LoginUserRequestDTO creds)
    {
      LoginUserResponseDTO loginRes = await _authService.Login(creds);

      Response.Headers.Append("x-access-token", loginRes.AccessToken);
      return Ok(loginRes);
    }
  }
}