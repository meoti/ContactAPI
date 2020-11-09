using System;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Data.Repository.Implementation;
using ContactAPI.Data.Repository.Interfaces;

namespace ContactAPI.Data.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ContactsDBContext _contactsContext;
        private IContactRepository _contacts;
        private IUserRepository _users;
        private ISkillRepository _skills;
        private IContactSkillRepository _contactSkills;

        public RepositoryWrapper(ContactsDBContext dBContext)
        {
            _contactsContext = dBContext;
        }

        public IContactRepository Contacts
        {
            get
            {
                if ( _contacts == null )
                {
                    _contacts = new ContactRepository(_contactsContext);
                }
                return _contacts;
            }
        }

        public ISkillRepository Skills
        {
            get
            {
                if ( _skills == null )
                {
                    _skills = new SkillRepository(_contactsContext);
                }
                return _skills;
            }
        }

        public IContactSkillRepository ContactSkills
        {
            get
            {
                if ( _contactSkills == null )
                {
                    _contactSkills = new ContactSkillRepository(_contactsContext);
                }
                return _contactSkills;
            }
        }

        public IUserRepository Users
        {
            get
            {
                if ( _users == null )
                {
                    _users = new UserRepository(_contactsContext);
                }
                return _users;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _contactsContext.SaveChangesAsync();
        }
    }
}
