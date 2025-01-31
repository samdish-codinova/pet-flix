using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
  {
    protected DbContext Context;

    public Repository(DbContext context)
    {
      Context = context;
    }

    public TEntity Get(int id)
    {
      return Context.Set<TEntity>().Find(id);
    }

    public IEnumerable<TEntity> GetAll()
    {
      return Context.Set<TEntity>().ToList();
    }

    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
    {
      return Context.Set<TEntity>().Where(expression);
    }

    public void Add(TEntity entity)
    {
      Context.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
      Context.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
      Context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
      Context.Set<TEntity>().RemoveRange();
    }
  }
}