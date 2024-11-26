using System.Linq.Expressions;

namespace InventoryManager.API.Handlers
{
    public class RequestHandler<TEntity> : IRequestHandler<TEntity>
    {

        public Expression<Func<TEntity, bool>> CreateFilterExpression(string filter)
        {
            var parts = filter.Split('=');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid filter format.");
            }

            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, parts[0]);
            var constant = Expression.Constant(Convert.ChangeType(parts[1], property.Type));
            var equal = Expression.Equal(property, constant);

            return Expression.Lambda<Func<TEntity, bool>>(equal, parameter);
        }

        public Expression<Func<TEntity, object>> CreateIncludeExpression(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<TEntity, object>>(property, parameter);
            return lambda;
        }
    }
}
