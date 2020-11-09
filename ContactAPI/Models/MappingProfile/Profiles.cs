
using AutoMapper;

using ContactAPI.Data.Models;
using ContactAPI.Models.DTO;

namespace ContactAPI.Models.MappingProfile
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<RegisterModel, User>();

            CreateMap<ContactRequestModel, Contact>()
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex.ToString()))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()));

            CreateMap<ContactPutRequestModel, Contact>()
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => src.Sex.ToString()))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()));

            CreateMap<Contact, ContactDTO>();

            CreateMap<ContactSkill, ContactInfoDTO>();
            CreateMap<Skill, SkillContactDTO>()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Expertise.ToString()));

            CreateMap<ContactSkill, SkillDTO>();

            CreateMap<SkillRequestModel, Skill>()
                .ForMember(dest => dest.Expertise, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToUpperInvariant()));

            CreateMap<SkillPutRequestModel, Skill>()
               .ForMember(dest => dest.Expertise, opt => opt.MapFrom(src => src.Level));
        }
    }
}
