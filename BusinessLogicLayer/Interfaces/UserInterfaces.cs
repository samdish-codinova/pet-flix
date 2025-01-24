using Models;

namespace BusinessLogicLayer {
  public interface IUserService {
    Task<User> CreateUserAsync(User user);
  }
}