using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Models;

namespace ContactAPI.Data.Repository.Interfaces
{
    public interface ISkillRepository : IRepositoryBase<Skill>
    {
        void DeleteSkills(IEnumerable<Skill> skills);

        Task<Skill> GetSkillWithContactSkillsByUserIdConditons(Guid userId, Expression<Func<Skill, bool>> expression);

        Task<Skill> GetSkillWithContactSkillsById(Guid id, Guid contactId);

        Task<Skill> GetSkillWithContactSkillsById(Guid id);

        Task<IEnumerable<Skill>> GetSkillsWithContactsAsync(PagingOptions pagingOptions);

        Task<IEnumerable<Skill>> GetSkillsWithContactByLevelAsync(ContactEnum.Expertise level, PagingOptions pagingOptions);
    }
}
