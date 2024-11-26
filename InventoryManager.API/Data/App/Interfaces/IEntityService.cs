using InventoryManager.API.Data.App.Models.Types;
using System.Linq.Expressions;

namespace InventoryManager.API.Data.App.Interfaces
{
    public interface IEntityService<T>
    {
        List<T> ListAll(
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[] includes);
        List<Y> ListAll<Y>(
            Expression<Func<Y, bool>>? filter = null,
            params Expression<Func<Y, object>>[] includes) where Y : class, IServiceable;
        T? GetById(int id, List<T>? list = null);
        bool CheckIfExist(string name);
        object? CrudOperation(T entity, CrudActions actionType);
        void SetTenantId(int tenantId);
    }
}
