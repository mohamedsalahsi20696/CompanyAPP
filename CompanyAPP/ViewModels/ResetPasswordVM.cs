using System.ComponentModel.DataAnnotations;

namespace CompanyAPP.ViewModels
{
    public class ResetPasswordVM
    {
        [Required(ErrorMessage = "New Password is Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm Password Does not match New Password")]
        public string ConfirmPassword { get; set; }
    }
}
