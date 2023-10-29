using AutoMapper;
using CompanyAPP.ViewModels;
using DataAccessLayer.Models;

namespace CompanyAPP.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserVM>().ReverseMap();
        }
    }
}
