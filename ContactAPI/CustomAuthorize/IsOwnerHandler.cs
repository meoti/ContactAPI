using System;
using System.Linq;
using System.Threading.Tasks;

using ContactAPI.Data.Models;
using ContactAPI.Models;
using ContactAPI.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ContactAPI.Attributes
{
    public class IsOwnerHandler : AuthorizationHandler<EditOwnerPermision>
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsOwnerHandler(IServiceWrapper serviceWrapper, IHttpContextAccessor httpContextAccessor)
        {
            _serviceWrapper = serviceWrapper;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EditOwnerPermision requirement)
        {
            string subId = context.User.Claims.SingleOrDefault(p => p.Type.ToLower() == "id")?.Value;
            if ( subId == null )
            {
                return Task.CompletedTask;
            }

            bool isOwner = false;
            switch ( requirement.Entity )
            {
                case ContactEnum.TrackableTypes.Contacts:
                    isOwner = IsContactOwnerAsync(subId);
                    break;
                case ContactEnum.TrackableTypes.Skills:
                    isOwner = IsSkillOwnerAsync(subId);
                    break;
                default:
                    break;
            }

            if ( isOwner )
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool IsContactOwnerAsync(string subId)
        {
            Guid id = new Guid(_httpContextAccessor.HttpContext.Request.RouteValues["id"].ToString());
            Contact contact = _serviceWrapper.ContactService.GetContactByIdAsync(id).GetAwaiter().GetResult();
            bool isCreator = contact.CreatedBy.ToString().Equals(subId, StringComparison.OrdinalIgnoreCase);

            return isCreator;
        }

        private bool IsSkillOwnerAsync(string subId)
        {
            Guid contactId = new Guid(_httpContextAccessor.HttpContext.Request.RouteValues["contactId"].ToString());
            Guid id = new Guid(_httpContextAccessor.HttpContext.Request.Query["id"].ToString());

            Skill skillWithContact = _serviceWrapper.SkillService.GetSkillWithContactsByIdAsync(id, contactId).GetAwaiter().GetResult();
            bool isCreator = skillWithContact.Contacts.Any(cs => cs.CreatedBy.ToString().Equals(subId, StringComparison.OrdinalIgnoreCase));

            return isCreator;
        }
    }

    public class EditOwnerPermision : IAuthorizationRequirement
    {
        public ContactEnum.TrackableTypes Entity { get; }

        public EditOwnerPermision(ContactEnum.TrackableTypes entity)
        {
            Entity = entity;
        }
    }
}
