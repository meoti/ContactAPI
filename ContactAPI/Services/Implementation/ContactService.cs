using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Data.Repository;
using ContactAPI.Models;
using ContactAPI.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Services.Implementation
{
    public class ContactService : IContactService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContactService(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryWrapper = repositoryWrapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Results<Contact>> UpsetContactAsync(Contact contact)
        {
            Results<Contact> results = new Results<Contact>() { Errors = Array.Empty<string>(), Succeded = false };
            IEnumerable<Claim> claims = _httpContextAccessor.HttpContext.User.Claims;
            contact.InsertDBDateTrackingInfo(claims);
            if ( contact.Id == Guid.Empty )
            {
                if ( ContactExists(contact) )
                {
                    results.Errors = results.Errors.Append("Duplicate contact not allowed ");
                    return results;
                }
                contact.Id = Guid.NewGuid();

                _repositoryWrapper.Contacts.Create(contact);
            }
            else
            {
                _repositoryWrapper.Contacts.Update(contact);
            }

            try
            {
                await _repositoryWrapper.SaveAsync();
            }
            catch ( DbUpdateException )
            {
                results.Errors = results.Errors.Append("A database error was produced whiles saving data ");
                return results;
            }

            results.Value = contact;
            results.Succeded = true;
            return results;

        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(PagingOptions pagingOptions)
        {
            return await _repositoryWrapper.Contacts.GetContactsWithSkillsAsync(pagingOptions);
        }

        public async Task<Contact> GetContactByIdAsync(Guid id)
        {
            Contact contact = await _repositoryWrapper.Contacts
                .GetContactsWithSkillsByIdAsync(id);
            return contact;
        }


        public async Task<IEnumerable<Contact>> GetContactsByUserIdAsync(Guid userId, PagingOptions pagingOptions)
        {
            IEnumerable<Contact> contacts = await _repositoryWrapper.Contacts
                .GetContactsWithSkillsByConditionAsync(x => x.CreatedBy.Equals(userId), pagingOptions);

            return contacts;
        }


        public async Task<Results<Contact>> DeleteContactByIdAsync(Guid id)
        {
            Results<Contact> results = new Results<Contact>() { Errors = Array.Empty<string>(), Succeded = false };
            Contact contact = await _repositoryWrapper.Contacts.GetContactsWithSkillsByIdAsync(id);

            if ( contact == null )
            {
                return null;
            }
            ICollection<ContactSkill> contactSkills = contact.Skills;
            if ( contactSkills.Any() )
            {
                _repositoryWrapper.ContactSkills.DeleteContactSkills(contactSkills);

            }

            _repositoryWrapper.Contacts.Delete(contact);

            try
            {
                await _repositoryWrapper.SaveAsync();
            }
            catch ( DbUpdateException )
            {
                results.Errors = results.Errors.Append("A database error was produced whiles saving data ");
                return null;
            }

            results.Value = contact;
            results.Succeded = true;
            return results;

        }

        private bool ContactExists(Contact partialContact)
        {
            return _repositoryWrapper.Contacts
                    .FindByConditionNoTracking(x => x.FirstName == partialContact.FirstName &&
                    x.LastName == partialContact.LastName &&
                    x.Email == partialContact.Email)
                    .Any();
        }
    }
}
