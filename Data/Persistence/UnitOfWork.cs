using DAL.EF.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.EF.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        
        public int Complete()
        {
            return _db.SaveChanges();
        }
        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await _db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
