using ContactAPI.Data.Repository;
using ContactAPI.Services.Implementation;
using ContactAPI.Services.Interfaces;

using Microsoft.AspNetCore.Http;

namespace ContactAPI.Services
{
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IUserService _userService;
        private IContactService _constactService;
        private SkillService _skillService;
        public IUserService UserService
        {
            get
            {
                if ( _userService == null )
                {
                    _userService = new UserService(_repositoryWrapper);
                }
                return _userService;
            }
        }

        public IContactService ContactService
        {
            get
            {
                if ( _constactService == null )
                {
                    _constactService = new ContactService(_repositoryWrapper, _httpContextAccessor);
                }
                return _constactService;
            }
        }

        public ISkillService SkillService
        {
            get
            {
                if ( _skillService == null )
                {
                    _skillService = new SkillService(_repositoryWrapper, _httpContextAccessor);
                }
                return _skillService;
            }
        }

        public ServiceWrapper(IRepositoryWrapper repositoryWrapper, IHttpContextAccessor httpContextAccessor)
        {
            _repositoryWrapper = repositoryWrapper;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
