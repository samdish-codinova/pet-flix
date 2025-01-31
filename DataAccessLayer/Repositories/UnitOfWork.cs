using DataAccessLayer.Data;

namespace DataAccessLayer.Repositories
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly ApplicationDBContext _dbContext;
    public UnitOfWork(ApplicationDBContext dbContext)
    {
      _dbContext = dbContext;
      Movies = new MovieRepository(_dbContext);
      Users = new UserRepository(_dbContext);
      Rentals = new RentalRepository(_dbContext);
    }

    public IMovieRepository Movies { get; private set; }
    public IUserRepository Users { get; private set; }
    public IRentalRepository Rentals { get; private set; }

    public int Complete()
    {
      return _dbContext.SaveChanges();
    }

    public void Dispose()
    {
      _dbContext.Dispose();
    }
  }
}