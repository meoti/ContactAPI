using System.Threading.Tasks;

using ContactAPI.Data.Repository.Interfaces;

namespace ContactAPI.Data.Repository
{
    public interface IRepositoryWrapper
    {
        IContactRepository Contacts { get; }

        ISkillRepository Skills { get; }

        IContactSkillRepository ContactSkills { get; }


        IUserRepository Users { get; }

        Task<int> SaveAsync();
    }
}
