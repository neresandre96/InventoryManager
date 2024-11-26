using System.Linq.Expressions;

namespace InventoryManager.API.Handlers
{
    public interface IRequestHandler<TEntity>
    {
        Expression<Func<TEntity, bool>> CreateFilterExpression(string filter);
        Expression<Func<TEntity, object>> CreateIncludeExpression(string propertyName);
    }
}
