namespace DataAccessLayer.Repositories
{
  public interface IUnitOfWork : IDisposable
  {
    IMovieRepository Movies { get; }
    int Complete();
  }
}