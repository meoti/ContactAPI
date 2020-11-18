using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

using ContactAPI.Data.Models;
using ContactAPI.Data.Repository;
using ContactAPI.Models;
using ContactAPI.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Services.Implementation
{
    public class SkillService : ISkillService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SkillService(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryWrapper = repositoryWrapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Skill> GetSkillWithContactsByIdAsync(Guid id, Guid contactId)
        {
            IEnumerable<Claim> claim = _httpContextAccessor.HttpContext.User.Claims;
            Guid subId = new Guid(claim.SingleOrDefault(p => p.Type.ToLower() == "id")?.Value);

            return await _repositoryWrapper.Skills.GetSkillWithContactSkillsById(id, contactId);
        }

        public async Task<Results<Skill>> CreateSkillAsync(Guid contactId, Skill skill)
        {
            Results<Skill> results = new Results<Skill>() { Errors = Array.Empty<string>(), Succeded = false };
            IEnumerable<Claim> claims = _httpContextAccessor.HttpContext.User.Claims;
            Skill skillExist = await SkillExisting(skill);
            if ( skill.Id == Guid.Empty )
            {
                if ( skillExist == null )
                {
                    InsertContactSkills(contactId, skill);
                    skill.Contacts.First().InsertDBDateTrackingInfo(claims);
                    _repositoryWrapper.Skills.Create(skill);
                }
                else
                {
                    ContactSkill contactSkill = new ContactSkill { SkillId = skillExist.Id, ContactId = contactId, Id = Guid.NewGuid() };
                    contactSkill.InsertDBDateTrackingInfo(claims);
                    _repositoryWrapper.ContactSkills.Create(contactSkill);
                }
            }

            await _repositoryWrapper.SaveAsync();

            results.Value = skill;
            results.Succeded = true;
            return results;
        }

        public async Task<Results<Skill>> UpdateSkillAsync(Skill skill)
        {
            Results<Skill> results = new Results<Skill>() { Errors = Array.Empty<string>(), Succeded = false };
            IEnumerable<Claim> claims = _httpContextAccessor.HttpContext.User.Claims;
            Skill skillExist = await SkillExisting(skill);
            ContactSkill contactSkill = skill.Contacts.FirstOrDefault();

            using (
                TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled)
            )
            {
                _repositoryWrapper.ContactSkills.Delete(contactSkill);
                await _repositoryWrapper.SaveAsync();

                contactSkill.InsertDBDateTrackingInfo(claims);
                if ( skillExist == null )
                {
                    skill.Id = Guid.NewGuid();
                    skill.Contacts = null;
                    _repositoryWrapper.Skills.Create(skill);
                    contactSkill.SkillId = skill.Id;
                }
                else
                {
                    contactSkill.SkillId = skillExist.Id;
                    contactSkill.Skill = null;
                    contactSkill.Contact = null;
                }

                _repositoryWrapper.ContactSkills.Create(contactSkill);
                await _repositoryWrapper.SaveAsync();

                transaction.Complete();
            }

            results.Value = skill;
            results.Succeded = true;
            return results;
        }

        public async Task<IEnumerable<Skill>> GetSkillsAsync(PagingOptions pagingOptions)
        {
            return await _repositoryWrapper.Skills.GetSkillsWithContactsAsync(pagingOptions);
        }

        public async Task<Results<Skill>> DeleteContactSkillAsync(Guid id, Guid contactId)
        {
            Results<Skill> results = new Results<Skill>() { Errors = Array.Empty<string>(), Succeded = false };
            Skill skillWithContact = await _repositoryWrapper.Skills.GetSkillWithContactSkillsById(id, contactId);

            if ( skillWithContact == null )
            {
                return null;
            }
            ICollection<ContactSkill> contactSkills = skillWithContact.Contacts;
            if ( contactSkills.Any() )
            {
                _repositoryWrapper.ContactSkills.DeleteContactSkills(contactSkills);

            }

            try
            {
                await _repositoryWrapper.SaveAsync();
            }
            catch ( DbUpdateException )
            {
                results.Errors = results.Errors.Append("A database error was produced whiles saving data ");
                return null;
            }

            results.Value = skillWithContact;
            results.Succeded = true;
            return results;
        }

        private static void InsertContactSkills(Guid contactId, Skill skill)
        {
            skill.Id = Guid.NewGuid();
            ContactSkill contactSkill = new ContactSkill { ContactId = contactId, SkillId = skill.Id, Id = Guid.NewGuid() };
            skill.Contacts.Add(contactSkill);
        }

        private async Task<Skill> SkillExisting(Skill partialSkill)
        {
            return await _repositoryWrapper.Skills
                    .FindByConditionNoTracking(x => x.Name == partialSkill.Name &&
                    x.Expertise == partialSkill.Expertise).SingleOrDefaultAsync();
        }

        public async Task<Skill> GetSkillWithContactsAsync(Guid id)
        {
            return await _repositoryWrapper.Skills.GetSkillWithContactSkillsById(id);
        }

        public async Task<int> DeleteSkillByIdAsync(Skill skill)
        {
            _repositoryWrapper.Skills.Delete(skill);
            return await _repositoryWrapper.SaveAsync();
        }


        public async Task<IEnumerable<Skill>> GetSkillsWithContactByLevelAsync(ContactEnum.Expertise level, PagingOptions pagingOptions)
        {
            return await _repositoryWrapper.Skills.GetSkillsWithContactByLevelAsync(level, pagingOptions);
        }
    }
}
