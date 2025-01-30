using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using PasswordHashing;

namespace BusinessLogicLayer
{
  public class AuthService : IAuthService
  {
    private ApplicationDBContext _dbContext;

    public AuthService(ApplicationDBContext dBContext)
    {
      _dbContext = dBContext;
    }

    public async Task<User> RegisterUser(RegisterUserRequestDTO userData)
    {
      PasswordHasher.CreatePasswordHash(userData.Password, out byte[] passwordSalt, out byte[] passwordHash);
      if (string.IsNullOrEmpty(userData.Name) || string.IsNullOrEmpty(userData.Email))
      {
        throw new InvalidData();
      }

      var userInDb = await _dbContext.User.FirstOrDefaultAsync(user => user.Email.ToLower() == userData.Email.Trim().ToLower());
      if (userInDb != null) {
        throw new ArgumentException("User with email already exists.");
      }

      var user = new User()
      {
        Name = userData.Name.Trim(),
        Email = userData.Email.Trim(),
        Role = UserRole.Customer,
        PasswordSalt = Convert.ToBase64String(passwordSalt),
        PasswordHash = Convert.ToBase64String(passwordHash),
        CreatedAt = DateTime.UtcNow
      };

      await _dbContext.User.AddAsync(user);
      int savedChanges = await _dbContext.SaveChangesAsync();
      if (savedChanges == 0)
      {
        throw new ApplicationException("Could not save changes to the DB.");
      }

      return user;
    }
  }
}