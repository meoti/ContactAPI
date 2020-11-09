using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Data.Repository.Interfaces;

namespace ContactAPI.Data.Repository.Implementation
{
	public class UserRepository : RepositoryBase<User>, IUserRepository
	{
		public UserRepository(ContactsDBContext contactsDBContext) : base(contactsDBContext)
		{

		}
	}
}
