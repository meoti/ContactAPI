using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Models;

namespace ContactAPI.Data.Repository.Interfaces
{
    public interface IContactRepository : IRepositoryBase<Contact>
    {
        Task<Contact> GetContactsWithSkillsByIdAsync(Guid id);

        Task<IEnumerable<Contact>> GetContactsWithSkillsAsync(PagingOptions pagingOptions);

        Task<IEnumerable<Contact>> GetContactsWithSkillsByConditionAsync(Expression<Func<Contact, bool>> expression, PagingOptions pagingOptions);
    }
}
