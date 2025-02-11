using DapperExtension.Infrastructure;
using System.Data;

namespace DapperExtension.Persistence;

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
