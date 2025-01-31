using DataAccessLayer.Data;
using Models;

namespace DataAccessLayer.Repositories
{
  public class MovieRepository : Repository<Movie>, IMovieRepository
  {
    public MovieRepository(ApplicationDBContext dbContext) : base(dbContext)
    {
    }

    public ApplicationDBContext DbContext
    {
      get { return Context as ApplicationDBContext; }
    }
  }
}