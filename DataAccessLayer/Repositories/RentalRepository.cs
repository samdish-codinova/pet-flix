using DataAccessLayer.Data;
using Models;

namespace DataAccessLayer.Repositories
{
  public class RentalRepository : Repository<Rental>, IRentalRepository
  {
    public RentalRepository(ApplicationDBContext dBContext) : base(dBContext)
    {

    }

    public ApplicationDBContext DBContext
    {
      get { return Context as ApplicationDBContext; }
    }
  }
}