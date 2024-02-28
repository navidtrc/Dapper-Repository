using DAL.Dapper;
using DAL.Dapper.Infrastructure;
using DAL.Dapper.Infrastructure.Repositories;
using DAL.Dapper.Persistence.Repositories;
using System.Data;

namespace DAL.Dapper.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DapperContext _db;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public UnitOfWork(DapperContext db)
        {
            _db = db;
            _connection = _db.CreateConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        private IPostRepository _posts;
        public IPostRepository Post => _posts ??= new PostRepository(_transaction);

        public void Complete()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
        }
        private void resetRepositories()
        {
            _posts = null;
        }

        public void Dispose()
        {
            _transaction.Dispose();
            _transaction = null;
            _connection.Dispose();
            _connection = null;
            GC.SuppressFinalize(this);
        }
    }
}
