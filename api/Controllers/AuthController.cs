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
      User user;
      try
      {
        user = await _authService.RegisterUser(userData);
      }
      catch (InvalidData)
      {
        return BadRequest("Please provide correct data.");
      }
      catch (ArgumentException)
      {
        return BadRequest("User with email already exists.");
      }

      return Ok(new { id = user.Id, name = user.Name, email = user.Email, createdAt = user.CreatedAt });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponseDTO>> Login([FromBody] LoginUserRequestDTO creds)
    {
      LoginUserResponseDTO loginRes;
      try
      {
        loginRes = await _authService.Login(creds);
      }
      catch (InvalidData)
      {
        return BadRequest("Please provide email and password in correct format.");
      }
      catch (AuthenticationException)
      {
        return Unauthorized("Incorrect email or password");
      }

      Response.Headers.Append("x-auth-token", loginRes.AccessToken);

      return Ok(loginRes);
    }
  }
}