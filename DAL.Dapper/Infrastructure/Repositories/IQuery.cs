using Entities;

namespace DAL.Dapper.Infrastructure.Repositories
{
    public interface IQuery<TEntity> where TEntity : class, IEntity
    {
        public string TableName { get; }
        string SelectQuery(string? query);
        string SelectByIdQuery(int id, string? query);
        string InsertQuery();
        string DeleteQuery();
        string UpdateQuery();
    }
}
