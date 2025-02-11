using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using DapperExtension.Infrastructure.Repositories;

namespace DapperExtension.Persistence.Repositories;

public class Query<TEntity> : IQuery<TEntity>
    where TEntity : class
{
    public Query()
    {
        TEntity instance = (TEntity)Activator.CreateInstance(typeof(TEntity));
        //_tableName = $"[{instance.SchemaName}].[{instance.TableName}]";
        _tableName = $"[{nameof(instance)}]";
    }

    private readonly string _tableName;
    public string TableName
    {
        get { return _tableName; }
    }

    public string SelectQuery(string? query)
    {
        string whereCondition = query.ToLower().Contains("where") ? "AND" : string.Empty;
        return $"SELECT * FROM {_tableName} {query} {whereCondition} WHERE IsDeleted=0";
    }
    public string SelectByIdQuery(int id, string? query)
    {
        string whereCondition = query.ToLower().Contains("where") ? "AND" : string.Empty;
        return $"SELECT * FROM {_tableName} WHERE {GetKeyColumnName()} = '{id}' AND {query} {whereCondition} WHERE IsDeleted=0";
    }
    public string InsertQuery() =>
        $"INSERT INTO {_tableName} ({GetColumns(true)}) VALUES ({GetPropertyNames(true)})";
    public string DeleteQuery() =>
        $"DELETE FROM {_tableName} WHERE {GetKeyColumnName()} = @{GetKeyPropertyName()}";
    public string UpdateQuery()
    {
        StringBuilder query = new StringBuilder();
        query.Append($"UPDATE {_tableName} SET ");
        foreach (var property in GetProperties(true))
        {
            var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

            string propertyName = property.Name;
            string columnName = columnAttr.Name;

            query.Append($"{columnName} = @{propertyName},");
        }
        query.Remove(query.Length - 1, 1);
        query.Append($" WHERE {GetKeyColumnName()} = @{GetKeyPropertyName()}");
        return query.ToString();
    }

    private IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null);

        return properties;
    }
    private string GetKeyPropertyName()
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

        if (properties.Any())
            return properties.FirstOrDefault().Name;

        return null;
    }
    private string GetColumns(bool excludeKey = false)
    {
        var type = typeof(TEntity);
        var columns = string.Join(", ", type.GetProperties()
            .Where(p => !excludeKey || !p.IsDefined(typeof(KeyAttribute)))
            .Where(p => !p.IsDefined(typeof(NotMappedAttribute)))
            .Select(p =>
            {
                var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                return columnAttr != null ? columnAttr.Name : p.Name;
            }));
        return columns;
    }
    private string GetPropertyNames(bool excludeKey = false)
    {
        var properties = typeof(TEntity).GetProperties()
            .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null)
            .Where(p => !p.IsDefined(typeof(NotMappedAttribute)));

        var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
        return values;
    }
    private string GetKeyColumnName()
    {
        PropertyInfo[] properties = typeof(TEntity).GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

            if (keyAttributes != null && keyAttributes.Length > 0)
            {
                object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                if (columnAttributes != null && columnAttributes.Length > 0)
                {
                    ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                    return columnAttribute.Name;
                }
                else
                {
                    return property.Name;
                }
            }
        }
        return null;
    }
}
