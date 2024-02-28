using System;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.EF.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
       
        int Complete();
        Task<int> CompleteAsync(CancellationToken cancellationToken);
    }
}
