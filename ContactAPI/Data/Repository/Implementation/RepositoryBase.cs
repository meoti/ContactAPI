using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ContactAPI.Data.Models;

using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Data.Repository.Implementation
{
	public class RepositoryBase<T> where T : class
	{
        protected ContactsDBContext ContactsDBContext { get; set; }
        public RepositoryBase(ContactsDBContext contactsDBContext)
        {
            ContactsDBContext = contactsDBContext;
        }

        public IQueryable<T> FindAll()
        {
            return ContactsDBContext.Set<T>();
        }

        public IQueryable<T> FindAllNoTracking()
        {
            return ContactsDBContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return ContactsDBContext.Set<T>().Where(expression);
        }

        public IQueryable<T> FindByConditionNoTracking(Expression<Func<T, bool>> expression)
        {
            return ContactsDBContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            ContactsDBContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            ContactsDBContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            ContactsDBContext.Set<T>().Remove(entity);
        }
    }
}
