using DataAccessLayer.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace CompanyAPP.ViewModels
{
    public class DepartmentVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code Is Required !!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name Is Required !!")]
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime? DateOfCreation { get; set; } = DateTime.Now;
        [InverseProperty("Department")]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
