using DataAccessLayer.Data;
using Models;

namespace DataAccessLayer.Repositories
{
  public class UserRepository : Repository<User>, IUserRepository
  {
    public UserRepository(ApplicationDBContext dBContext) : base(dBContext)
    {

    }

    public ApplicationDBContext DbContext
    {
      get { return Context as ApplicationDBContext; }
    }
  }
}