using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Models;

namespace ContactAPI.Services.Interfaces
{
    public interface IContactService
    {
        Task<Results<Contact>> UpsetContactAsync(Contact contact);

        Task<Contact> GetContactByIdAsync(Guid id);

        Task<Results<Contact>> DeleteContactByIdAsync(Guid id);

        Task<IEnumerable<Contact>> GetContactsAsync(PagingOptions pagingOptions);

        Task<IEnumerable<Contact>> GetContactsByUserIdAsync(Guid userId, PagingOptions pagingOptions);
    }
}
