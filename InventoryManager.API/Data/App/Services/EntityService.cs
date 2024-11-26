using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Data.App.Models.Types;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManager.API.Data.App.Services
{
    public abstract class EntityService<TEntity, TContext>(TContext context) :
    IEntityService<TEntity>
    where TEntity : class, IServiceable
    where TContext : IAppDbContext
    {

        protected readonly TContext _context = context;

        public void SetTenantId(int tenantId)
        {
            _context.SetTenantId(tenantId);
        }

        public virtual List<TEntity> ListAll(
            Expression<Func<TEntity, bool>>? filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return ProcessIncludes(includes, filter);
        }

        public virtual List<T> ListAll<T>(
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[] includes) where T : class, IServiceable
        {
            return ProcessIncludes(includes, filter);
        }

        private List<T> ProcessIncludes<T>(
            Expression<Func<T, object>>[] includes,
            Expression<Func<T, bool>>? filter = null) where T : class, IServiceable
        {
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return [.. query];
        }

        public virtual TEntity? GetById(int id, List<TEntity>? entities = null) =>
            entities == null ? ListAll().FirstOrDefault(
                e => e.ID == id) : entities.FirstOrDefault(e => e.ID == id);

        public virtual bool CheckIfExist(string name) =>
            ListAll().FirstOrDefault(e => e.Name
            .Equals(name, StringComparison.CurrentCultureIgnoreCase)) != null;


        public virtual object? CrudOperation(TEntity entity, CrudActions actionType)
        {
            if (entity == null) return null;

            var dbSet = _context.Set<TEntity>();

            if (dbSet == null) return null;

            switch (actionType)
            {
                case CrudActions.Add:
                    dbSet.Add(entity);
                    break;
                case CrudActions.Update:
                    dbSet.Update(entity);
                    break;
                case CrudActions.Remove:
                    dbSet.Remove(entity);
                    break;
                default:
                    throw new ArgumentException("Invalid action type", nameof(actionType));
            }

            try
            {
                _context.SaveChanges();

                return actionType == CrudActions.Remove ? null : entity;
            }
            catch (DbUpdateException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

    }
}
