using System.Resources;
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

      if (user is null)
      {
        throw new ApplicationException("User is null.");
      }

      return Ok(new { id = user.Id, name = user.Name, email = user.Email, createdAt = user.CreatedAt });
    }
  }
}