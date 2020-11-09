
using System.Collections.Generic;

using ContactAPI.Data.Models;

namespace ContactAPI.Data.Repository.Interfaces
{
    public interface IContactSkillRepository : IRepositoryBase<ContactSkill>
    {
        void DeleteContactSkills(IEnumerable<ContactSkill> contactsKills);
    }
}
