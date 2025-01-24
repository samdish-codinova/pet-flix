using DataAccessLayer.Data;
using Models;

namespace BusinessLogicLayer
{
  public class UserService : IUserService
  {
    private readonly ApplicationDBContext _dbContext;

    public UserService(ApplicationDBContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<User> CreateUserAsync(User user)
    {
      if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
      {
        throw new InvalidData();
      }

      await _dbContext.User.AddAsync(user);
      await _dbContext.SaveChangesAsync();

      return user;
    }
  }
}