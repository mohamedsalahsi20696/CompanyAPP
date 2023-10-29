using DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace CompanyAPP.ViewModels
{
    public class EmployeeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name Is Required !!")]
        [MaxLength(50, ErrorMessage = "Max Length Is 50 Chars !!")]
        [MinLength(5, ErrorMessage = "Min Length Is 5 Chars !!")]
        public string Name { get; set; }

        [Range(20, 50, ErrorMessage = "Age Must be In Range From 20 To 50")]
        public int Age { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address Must be Like 123-Streat-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        
        [ForeignKey("Department")]
        public int? DeptId { get; set; }
        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
