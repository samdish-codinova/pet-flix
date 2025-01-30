using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using PasswordHashing;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BusinessLogicLayer
{
  public class AuthService : IAuthService
  {
    private ApplicationDBContext _dbContext;
    private IConfiguration _config;

    public AuthService(ApplicationDBContext dBContext, IConfiguration configuration)
    {
      _dbContext = dBContext;
      _config = configuration;
    }

    public async Task<User> RegisterUser(RegisterUserRequestDTO userData)
    {
      PasswordHasher.CreatePasswordHash(userData.Password, out byte[] passwordSalt, out byte[] passwordHash);
      if (string.IsNullOrEmpty(userData.Name) || string.IsNullOrEmpty(userData.Email))
      {
        throw new InvalidData();
      }

      var userInDb = await _dbContext.User.FirstOrDefaultAsync(user => user.Email.ToLower() == userData.Email.Trim().ToLower());
      if (userInDb != null)
      {
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

    public async Task<LoginUserResponseDTO> Login(LoginUserRequestDTO creds)
    {
      if (string.IsNullOrEmpty(creds.Email) || creds.Password == null)
      {
        throw new InvalidData();
      }

      var user = await _dbContext.User.FirstOrDefaultAsync(user => user.Email.Trim().ToLower() == creds.Email.Trim().ToLower());

      if (user is null)
      {
        throw new AuthenticationException();
      }

      var passwordMatched = PasswordHasher.VerifyPasswordHash(creds.Password, Convert.FromBase64String(user.PasswordSalt), Convert.FromBase64String(user.PasswordHash));

      if (!passwordMatched)
      {
        throw new AuthenticationException();
      }

      string jwt = GenerateJwt(user);

      return new LoginUserResponseDTO()
      {
        AccessToken = jwt,
        User = new LoginUserDTO()
        {
          Id = user.Id,
          Name = user.Name,
          Email = user.Email,
          Role = user.Role,
          CreatedAt = user.CreatedAt,
          UpdatedAt = user.UpdatedAt,
        }
      };
    }

    private string GenerateJwt(User user)
    {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

      var claims = new[] {
        new Claim("id", user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Name, user.Name),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim("role", user.Role.ToString()),
        new Claim("createdAt", user.CreatedAt?.ToString())
      };

      var token = new JwtSecurityToken(
        _config["Jwt:Issuer"],
        _config["Jwt:Issuer"],
        claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
};