namespace DataAccessLayer.Repositories
{
  public interface IUnitOfWork : IDisposable
  {
    IMovieRepository Movies { get; }
    IUserRepository Users { get; }
    int Complete();
  }
}