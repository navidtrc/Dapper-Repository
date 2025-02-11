using Dapper;
using DapperExtension.Infrastructure.Repositories;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DapperExtension.Persistence.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected IDbTransaction Transaction { get; private set; }
    protected IDbConnection Connection => Transaction.Connection;

    protected readonly IQuery<TEntity> _query;
    public Repository(IDbTransaction transaction)
    {
        Transaction = transaction;

        _query = new Query<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAsync(string? query = null) => await Connection.QueryAsync<TEntity>(_query.SelectQuery(query), transaction: Transaction);
    public virtual async Task<IEnumerable<TEntity>> GetAsync<TProperty>(string? query = null, params Expression<Func<TEntity, TProperty>>[] referenceProperty)
    {
        StringBuilder builder = new StringBuilder();

        List<Type> types = new List<Type>();

        foreach (var property in referenceProperty)
        {
            MemberExpression memberExpression = (MemberExpression)property.Body;
            PropertyInfo propertyInfo = (PropertyInfo)memberExpression.Member;
            Type propertyType = propertyInfo.PropertyType;

            var relationInstance = (Activator.CreateInstance(propertyType)) as TEntity;
            //string relationTableName = $"[{relationInstance.SchemaName}].[{relationInstance.TableName}]";
            string relationTableName = $"[{nameof(relationInstance)}]";
            types.Add(relationInstance.GetType());

            builder.AppendLine("LEFT OUTER JOIN");
            builder.AppendLine(relationTableName);
            builder.AppendLine($"ON {relationTableName}.Id = {_query.TableName}.{nameof(TEntity)}Id");
        }

        Func<object[], TEntity> mapper = objects =>
        {
            var entity = (TEntity)objects[0];
            //foreach (var type in types)
            //{
            //}
            // MUST COMPLETE HERE
            return entity;
        };

        return await Connection.QueryAsync<TEntity>(_query.SelectQuery(builder.ToString()), mapper, transaction: Transaction);
    }


    public async Task<TEntity> GetByIdAsync(int id, string? query) => await Connection.QueryFirstOrDefaultAsync<TEntity>(_query.SelectByIdQuery(id, query), transaction: Transaction);
    public Task<IEnumerable<TEntity>> GetByIdAsync<TProperty>(int id, string? query, params Expression<Func<TEntity, TProperty>>[] referenceProperty)
    {
        throw new NotImplementedException();
    }

    public async Task<int> AddAsync(TEntity entity) => await Connection.ExecuteScalarAsync<int>(_query.InsertQuery(), transaction: Transaction);

    public async Task<int> UpdateAsync(TEntity entity) => await Connection.ExecuteAsync(_query.UpdateQuery(), entity, transaction: Transaction);

    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        int rowsEffected = await Connection.ExecuteAsync(_query.DeleteQuery(), entity, transaction: Transaction);
        return rowsEffected > 0 ? true : false;
    }

    public Task<object> SqlRaw(string sqlQuery, TEntity entity)
    {
        throw new NotImplementedException();
    }
    public Task<object> SqlRaw(string sqlQuery, object input)
    {
        throw new NotImplementedException();
    }
}
