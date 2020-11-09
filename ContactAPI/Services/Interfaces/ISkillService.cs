using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Models;

namespace ContactAPI.Services.Interfaces
{
    public interface ISkillService
    {
        Task<Results<Skill>> CreateSkillAsync(Guid contactId, Skill skill);

        Task<Results<Skill>> UpdateSkillAsync(Skill skill);

        Task<Skill> GetSkillWithContactsByIdAsync(Guid id, Guid contactId);
        
        Task<Skill> GetSkillWithContactsAsync(Guid id);

        Task<int> DeleteSkillByIdAsync(Skill skill);

        Task<Results<Skill>> DeleteContactSkillAsync(Guid id, Guid contactId);

        Task<IEnumerable<Skill>> GetSkillsAsync(PagingOptions pagingOptions);

        Task<IEnumerable<Skill>> GetSkillsWithContactByLevelAsync(ContactEnum.Expertise level, PagingOptions pagingOptions);
    }
}
