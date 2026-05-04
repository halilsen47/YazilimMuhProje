using Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositries.Interfaces
{
    public interface IRepository<T> where T : class, IEntitiy, new()
    {
        // Temel CRUD operasyonları
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        // Filtreleyerek tek bir veri çekmek için (Örn: Id'ye göre)
        T Get(Expression<Func<T, bool>> filter);

        // Tüm listeyi veya filtreli listeyi çekmek için
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
    }
}
