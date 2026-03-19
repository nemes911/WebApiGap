using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApiGap.DbServices.PostgresFactory;


namespace API_GAI.DbServices.DefaultCommand.Implements;
public class DefaultDb<Tentity> : IDefaultDB<Tentity> where Tentity : class
{
    private readonly PostgresContext _postgresContext;
    private readonly DbSet<Tentity> _dbSet;

    public DefaultDb(PostgresContextFactory factory)
    {
        _postgresContext = factory.Create();
        _dbSet = _postgresContext.Set<Tentity>();
    }

    public DefaultDb() { }

    public async Task<Tentity> AddAsync(Tentity t)
    {
        await _dbSet.AddAsync(t);
        await _postgresContext.SaveChangesAsync();
        return t;
    }

    public async Task<List<Tentity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<Tentity> UpdateAsync(Tentity t)
    {
        _dbSet.Update(t);
        await _postgresContext.SaveChangesAsync();
        return t;
    }

    public async Task<List<Tentity>> GetByAsync<TField>(
        Expression<Func<Tentity, TField>> fieldSelector,
        TField value
    )
    {
        
        var parameter = fieldSelector.Parameters[0]; 
        var body = Expression.Equal(fieldSelector.Body, Expression.Constant(value, typeof(TField)));

        var lambda = Expression.Lambda<Func<Tentity, bool>>(body, parameter);

        
        return await _dbSet.Where(lambda).ToListAsync();
    }
}