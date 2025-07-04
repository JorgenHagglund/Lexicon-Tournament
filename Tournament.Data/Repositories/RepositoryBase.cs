using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tournament.Core.Contracts;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories;

public class RepositoryBase<T>(TournamentContext context) : IRepositoryBase<T> where T : class
{
    protected DbSet<T> DbSet { get; } = context.Set<T>();
    public void Create(T entity) =>
        DbSet.Add(entity);

    public void Delete(T entity) =>
        DbSet.Remove(entity);

    public IQueryable<T> FindAll(bool trackChanges = false) =>
        trackChanges ? DbSet : DbSet.AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
        trackChanges ? DbSet.Where(expression) : DbSet.Where(expression).AsNoTracking();

    public void Update(T entity) =>
        DbSet.Update(entity);
}
