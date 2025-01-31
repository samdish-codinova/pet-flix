using DataAccessLayer.Data;
using Models;

namespace DataAccessLayer.Repositories
{
  public class MovieRepository : Repository<Movie>, IMovieRepository
  {
    MovieRepository(ApplicationDBContext dbContext) : base(dbContext)
    {
    }
  }
}