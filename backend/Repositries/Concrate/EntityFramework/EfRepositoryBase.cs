using Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositries.Concrate.EntityFramework
{
    public class EfRepositoryBase<TEntity, TContext> : IRepository<TEntity> where TEntity : class, IEntitiy, new() where TContext : DbContext
    {

        protected readonly TContext Context;

        public EfRepositoryBase(TContext context)
        {
            Context = context;
        }

        public void Add(TEntity entity)
        {
            var addedEntity = Context.Entry(entity);
            addedEntity.State = EntityState.Added;
            Context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            var deletedEntity = Context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return Context.Set<TEntity>().SingleOrDefault(filter);
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                            ? Context.Set<TEntity>().ToList()
                            : Context.Set<TEntity>().Where(filter).ToList();
        }

        public void Update(TEntity entity)
        {
            var updatedEntity = Context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
