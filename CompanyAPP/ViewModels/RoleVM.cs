using System;

namespace CompanyAPP.ViewModels
{
    public class RoleVM
    {
        public string Id { get; set; }
        public string RoleName { get; set; }

        public RoleVM()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
