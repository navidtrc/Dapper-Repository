namespace DapperExtension.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    void Complete();
}
