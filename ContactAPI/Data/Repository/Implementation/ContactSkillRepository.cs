
using System.Collections.Generic;

using ContactAPI.Data.Models;
using ContactAPI.Data.Repository.Interfaces;

namespace ContactAPI.Data.Repository.Implementation
{
    public class ContactSkillRepository : RepositoryBase<ContactSkill>, IContactSkillRepository
    {
        public ContactSkillRepository(ContactsDBContext contactsDBContext) : base(contactsDBContext)
        {

        }

        public void DeleteContactSkills(IEnumerable<ContactSkill> contactsKills)
        {
            ContactsDBContext.ContactSkills.RemoveRange(contactsKills);
        }
    }
}
