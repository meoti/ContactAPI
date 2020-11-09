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
    public class SkillRepository : RepositoryBase<Skill>, ISkillRepository
    {
        public SkillRepository(ContactsDBContext contactsDBContext) : base(contactsDBContext)
        {

        }

        public void DeleteSkills(IEnumerable<Skill> skills)
        {
            ContactsDBContext.Skills.RemoveRange(skills);
        }

        public async Task<Skill> GetSkillWithContactSkillsByUserIdConditons(Guid userId, Expression<Func<Skill, bool>> expression)
        {
            return await ContactsDBContext.Skills.Include(s => s.Contacts)
                    .Where(c => c.Contacts.Any(cs => cs.CreatedBy.Equals(userId)))
                .SingleOrDefaultAsync(expression);
        }

        public async Task<Skill> GetSkillWithContactSkillsById(Guid id, Guid contactId)
        {
            return await ContactsDBContext.Skills
                .Include(s => s.Contacts).ThenInclude(sc => sc.Contact)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Contacts.Any(cs => cs.ContactId.Equals(contactId)) && c.Id == id);
        }

        public async Task<Skill> GetSkillWithContactSkillsById(Guid id)
        {
            return await ContactsDBContext.Skills
                .Include(s => s.Contacts).ThenInclude(sc => sc.Contact)
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);
        }
        

        public async Task<IEnumerable<Skill>> GetSkillsWithContactByLevelAsync(ContactEnum.Expertise level, PagingOptions pagingOptions)
        {
            IQueryable<Skill> skillquery = ContactsDBContext.Skills
                .Include(s => s.Contacts).ThenInclude(sc => sc.Contact)
                .AsNoTracking()
                .Where(s => s.Expertise == (int) level);

            return await skillquery
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<Skill>> GetSkillsWithContactsAsync(PagingOptions pagingOptions)
        {
            IQueryable<Skill> skillquery = ContactsDBContext.Skills
                .Include(s => s.Contacts).ThenInclude(sc => sc.Contact)
                .AsNoTracking();

            return await skillquery
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ToArrayAsync();
        }
    }
}
