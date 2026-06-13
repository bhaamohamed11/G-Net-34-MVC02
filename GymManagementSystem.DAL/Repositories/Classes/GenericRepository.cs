<<<<<<< HEAD
﻿using GymManagementSystem.DAL.Data.DbContexts;
=======
﻿using GymManagementSystem.DAL.DbContexts;
>>>>>>> ab71e25af933999c52642f7d155ce0ca029030c0
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;


        public GenericRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync(ct);

        }

       

        public Task<int> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Remove(entity);
            return _dbContext.SaveChangesAsync(ct);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _dbSet.FindAsync([id] , ct);

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, ct);

        }
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
       => _dbSet.AnyAsync(predicate, ct);
<<<<<<< HEAD

        public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken ct = default)
       => predicate is null ? _dbSet.AsNoTracking().CountAsync(ct) : _dbSet.CountAsync(predicate, ct);
=======
        
>>>>>>> ab71e25af933999c52642f7d155ce0ca029030c0
    }
}
