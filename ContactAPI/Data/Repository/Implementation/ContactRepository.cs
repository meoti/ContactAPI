
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Data.Repository.Interfaces;
using ContactAPI.Models;

using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Data.Repository.Implementation
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(ContactsDBContext contactsDBContext) : base(contactsDBContext)
        {

        }

        public async Task<IEnumerable<Contact>> GetContactsWithSkillsAsync(PagingOptions pagingOptions)
        {
            IQueryable<Contact> contactsQuery = ContactsDBContext.Contacts
                .Include(u => u.CreatedUser)
                .Include(c => c.Skills)
                .ThenInclude(cs => cs.Skill);

            return await contactsQuery
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .AsNoTracking()
                .ToArrayAsync();
        }
        
        public async Task<Contact> GetContactsWithSkillsByIdAsync(Guid contactId)
        {
            return await ContactsDBContext.Contacts
                .Include(u => u.CreatedUser)
                .Include(c => c.Skills)
                .ThenInclude(cs => cs.Skill)
                .Where(x => x.Id == contactId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Contact>> GetContactsWithSkillsByConditionAsync(Expression<Func<Contact, bool>> expression, PagingOptions pagingOptions)
        {
            IQueryable<Contact> contactsQuery = ContactsDBContext.Contacts
                 .Include(u => u.CreatedUser)
                 .Include(c => c.Skills)
                 .ThenInclude(cs => cs.Skill)
                 .Where(expression);

            pagingOptions.Offset ??= 0;

            //int size = contactsQuery.Count(); // Make a resouceResponse object that includes size and items

            return await contactsQuery.AsNoTracking()
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArrayAsync();

        }

        public void DeleteContacts(IEnumerable<Contact> contacts)
        {
            ContactsDBContext.Contacts.RemoveRange(contacts);
        }
    }
}
