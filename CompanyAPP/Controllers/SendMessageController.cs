using CompanyAPP.Helpers;
using CompanyAPP.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompanyAPP.Controllers
{
    public class SendMessageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SendMessageController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(SendMessageVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var email = new Email()
                    {
                        Subject = "Message from teacher",
                        Body = model.Message,
                        To = user.Email
                    }; // helper function

                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Email is not valid");
            }

            return View(model);
        }

    }
}
