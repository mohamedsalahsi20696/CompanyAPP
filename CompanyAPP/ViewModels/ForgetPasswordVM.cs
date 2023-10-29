using System.ComponentModel.DataAnnotations;

namespace CompanyAPP.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
