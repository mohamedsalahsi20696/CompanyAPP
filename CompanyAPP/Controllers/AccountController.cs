using CompanyAPP.Helpers;
using CompanyAPP.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Threading.Tasks;

namespace CompanyAPP.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FName = model.FName,
                    LName = model.LName,
                    Email = model.Email,
                    UserName = model.Email.Split('@')[0],
                    IsAgree = model.IsAgree,
                };

                var res = await _userManager.CreateAsync(user, model.Password);
                // this function to hash password and to work should in startup class write services.AddIdentity
                // and AddEntityFrameworkStores implement inside it many function like (CreateAsync)

                if (res.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                {
                    foreach (var error in res.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (flag)
                    {
                        var res = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (res.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Password is Invalid");
                }
                else
                    ModelState.AddModelError(string.Empty, "Email is Invalid");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new { email = model.Email, token = token }, Request.Scheme);
                    //https://localhost:44377/Account/ResetPassword/email=mesho6567@gmail.com&token=nfsdnoidoidmoimoimomir

                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        Body = passwordResetLink,
                        To = user.Email
                    }; // helper function

                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Email is not valid");
            }

            return View(model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);

                var res = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (res.Succeeded)
                    return RedirectToAction(nameof(Login));

                foreach (var error in res.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

    }
}
