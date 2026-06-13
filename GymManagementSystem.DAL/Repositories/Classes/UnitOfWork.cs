using GymManagementSystem.DAL.Data.DbContexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        private readonly GymDbContext _dbContext;
        private readonly ISessionRepository sessionRepository;

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository) 
        {
            _dbContext = dbContext; 
            _SessionRepository = sessionRepository;
        }

        public ISessionRepository _SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if(_repositories.TryGetValue(TypeName,out object? Value))
            {
                return (IGenericRepository<TEntity>)Value;

            }
            var Repo = new GenericRepository<TEntity>(_dbContext);
            _repositories[TypeName] = Repo;
            return Repo;

        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
      => _dbContext.SaveChangesAsync(ct);
    }
}
