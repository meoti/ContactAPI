using System;
using System.Linq;
using System.Linq.Expressions;

namespace ContactAPI.Data.Repository.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindAllNoTracking();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByConditionNoTracking(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
