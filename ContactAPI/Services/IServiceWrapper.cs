using ContactAPI.Services.Interfaces;

namespace ContactAPI.Services
{
    public interface IServiceWrapper
    {
        IUserService UserService { get; }

        IContactService ContactService { get; }

        ISkillService SkillService { get; }
    }
}
