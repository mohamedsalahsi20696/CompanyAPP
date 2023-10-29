using AutoMapper;
using CompanyAPP.ViewModels;
using DataAccessLayer.Models;

namespace CompanyAPP.MapperProfiles
{
    public class DepartmentProfile:Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentVM, Department>().ReverseMap();
        }
    }
}
