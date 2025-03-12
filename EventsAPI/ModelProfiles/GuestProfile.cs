using AutoMapper;
using EventsAPI.Models;
using Models;

namespace EventsAPI.ModelProfiles
{
    public class GuestProfile : Profile
    {
        public GuestProfile()
        {
            CreateMap<RegisterModel, Guest>()
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dst => dst.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.BirthDate, opt => opt.MapFrom(src => src.BirthDate));
        }
    }
}
