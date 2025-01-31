using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using PasswordHashing;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BusinessLogicLayer
{
  public class AuthService : IAuthService
  {
    private IConfiguration _config;
    private IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
      _unitOfWork = unitOfWork;
      _config = configuration;
    }

    public async Task<User> RegisterUser(RegisterUserRequestDTO userData)
    {
      if (userData.Password.Length == 0)
      {
        throw new ErrorResponseException("Password can not be empty.", HttpStatusCode.BadRequest);
      }

      if (string.IsNullOrWhiteSpace(userData.Name) || string.IsNullOrWhiteSpace(userData.Email))
      {
        throw new ErrorResponseException("Invalid name or email specified.", HttpStatusCode.BadRequest);
      }

      var userInDb = _unitOfWork.Users.Find(user => user.Email.ToLower() == userData.Email.Trim().ToLower());
      if (userInDb.Any())
      {
        throw new ErrorResponseException($"User with email  \"{userData.Email}\" already exists.", HttpStatusCode.BadRequest);
      }

      PasswordHasher.CreatePasswordHash(userData.Password, out byte[] passwordSalt, out byte[] passwordHash);

      var user = new User()
      {
        Name = userData.Name.Trim(),
        Email = userData.Email.Trim(),
        Role = UserRole.Customer,
        PasswordSalt = Convert.ToBase64String(passwordSalt),
        PasswordHash = Convert.ToBase64String(passwordHash),
        CreatedAt = DateTime.UtcNow
      };

      _unitOfWork.Users.Add(user);
      int savedChanges = _unitOfWork.Complete();
      if (savedChanges == 0)
      {
        throw new ErrorResponseException("Could not save data. Something went wrong.", HttpStatusCode.InternalServerError);
      }

      return user;
    }

    public async Task<LoginUserResponseDTO> Login(LoginUserRequestDTO creds)
    {
      if (string.IsNullOrWhiteSpace(creds.Email) || string.IsNullOrEmpty(creds.Password))
      {
        throw new ErrorResponseException("Invalid email or password specified.", HttpStatusCode.BadRequest);
      }

      var users = _unitOfWork.Users.Find(user => user.Email.Trim().ToLower() == creds.Email.Trim().ToLower());

      if (!users.Any())
      {
        throw new ErrorResponseException("Incorrect email or password provided", HttpStatusCode.Unauthorized);
      }

      var user = users.First(user => user.Email.Trim().ToLower() == creds.Email.Trim().ToLower());

      var passwordMatched = PasswordHasher.VerifyPasswordHash(creds.Password, Convert.FromBase64String(user.PasswordSalt), Convert.FromBase64String(user.PasswordHash));

      if (!passwordMatched)
      {
        throw new ErrorResponseException("Incorrect email or password provided", HttpStatusCode.Unauthorized);
      }

      string jwt = GenerateJwt(user);

      return new LoginUserResponseDTO()
      {
        AccessToken = jwt,
        User = new BaseUser()
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

    public string GenerateJwt(User user)
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