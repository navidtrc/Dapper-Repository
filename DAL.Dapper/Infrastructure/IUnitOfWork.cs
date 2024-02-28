using DAL.Dapper.Infrastructure.Repositories;

namespace DAL.Dapper.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository Post { get; }

        void Complete();
    }
}
