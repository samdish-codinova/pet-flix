using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
      var userInDb = await _userService.CreateUserAsync(user);

      return userInDb;
    }
  }

}