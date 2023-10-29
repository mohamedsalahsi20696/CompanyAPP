using AutoMapper;
using CompanyAPP.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace CompanyAPP.MapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleVM, IdentityRole>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.RoleName)).ReverseMap();
        }
    }
}