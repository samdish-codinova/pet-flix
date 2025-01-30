using Models;

namespace BusinessLogicLayer
{
  public interface IAuthService {
    Task<User> RegisterUser(RegisterUserRequestDTO userData);
    Task<LoginUserResponseDTO> Login(LoginUserRequestDTO creds);
  }
}
