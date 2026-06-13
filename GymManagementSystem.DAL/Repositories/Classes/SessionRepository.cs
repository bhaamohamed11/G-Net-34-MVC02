using GymManagementSystem.DAL.Data.DbContexts;
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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbcontext;
        public SessionRepository(GymDbContext dbcontext): base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

      

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<Session> query = _dbcontext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s=>s.Category);
            if(predicate != null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync(ct);
        }

        public Task GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountOfBookedSlotsAsync(int SessionId, CancellationToken ct = default)
        =>_dbcontext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == SessionId);

        public Task<Session?> GetSessionWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct = default)
       =>_dbcontext.Sessions.AsNoTracking().Include(s=>s.Trainer).Include(s=>s.Category).FirstOrDefaultAsync(s=>s.Id==SessionId);

    }
}
