using API_GAI.DbServices.DefaultCommand.Interface;
using API_GAI.DbServices.SRC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApiGap.DbServices.PostgresFactory;

namespace API_GAI.DbServices.DefaultCommand.Implements
{
    public class DefaultDb<Tentity> : IDefaultDB<Tentity> where Tentity : class
    {
        private readonly PostgresContext _postgresContext;

        private readonly DbSet<Tentity> _dbSet;

        public DefaultDb(PostgresContextFactory factory)
        {
            _postgresContext = factory.Create();
            _dbSet = _postgresContext.Set<Tentity>();
        }
        public async Task<Tentity> AddAsync(Tentity t)
        {
            await _dbSet.AddAsync(t);
            await _postgresContext.SaveChangesAsync();
            return t;
        }

        public async Task<List<Tentity>> GetAllAsync(params Expression<Func<Tentity, object>>[] includes)
        {
            IQueryable<Tentity> query = _dbSet;

            if (includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<Tentity> UpdateAsync(Tentity t)
        {
            _dbSet.Update(t);
            await _postgresContext.SaveChangesAsync();
            return t;
        }
    }
}
