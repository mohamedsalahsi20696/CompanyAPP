using System.ComponentModel.DataAnnotations;

namespace CompanyAPP.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "FName is Required")]
        public string FName { get; set; }

        [Required(ErrorMessage = "LName is Required")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password Does not match Password")]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
}
