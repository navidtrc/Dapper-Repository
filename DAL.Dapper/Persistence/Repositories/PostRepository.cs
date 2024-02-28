using Dapper;
using Entities;
using DAL.Dapper.Infrastructure.Repositories;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DAL.Dapper.Persistence.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public override async Task<IEnumerable<Post>> GetAsync<TProperty>(string? query = null, params Expression<Func<Post, TProperty>>[] referenceProperty)
        {
            MemberExpression memberExpression = (MemberExpression)referenceProperty[0].Body;
            PropertyInfo propertyInfo = (PropertyInfo)memberExpression.Member;
            Type propertyType = propertyInfo.PropertyType;

            var categoryInstance = (Activator.CreateInstance(propertyType)) as Category;
            string categoryTableName = $"[{categoryInstance.SchemaName}].[{categoryInstance.TableName}]";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("LEFT OUTER JOIN");
            stringBuilder.AppendLine(categoryTableName);
            stringBuilder.AppendLine($"ON {categoryTableName}.{nameof(categoryInstance.Id)} = {_query.TableName}.{nameof(Category)}Id");

            return await Connection.QueryAsync<Post, Category, Post>(_query.SelectQuery(stringBuilder.ToString()), (post, category) =>
            {
                post.Category = category;
                return post;
            }, transaction: Transaction);
        }
    }
}
