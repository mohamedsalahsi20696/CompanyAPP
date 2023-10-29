using AutoMapper;
using CompanyAPP.ViewModels;
using DataAccessLayer.Models;

namespace CompanyAPP.MapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeVM, Employee>().ReverseMap();

            //CreateMap<EmployeeVM, Employee>()
            //    .ForMember(e => e.Email, m => m.MapFrom(vm => vm.Emails))
            //    .ForMember(e => e.PhoneNumber, m => m.MapFrom(vm => vm.PhoneNumbers))
            //    .ReverseMap();
        }
    }
}
