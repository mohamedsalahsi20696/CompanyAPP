using AutoMapper;
using CompanyAPP.Helpers;
using CompanyAPP.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPP.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string email)
        {

            if (string.IsNullOrEmpty(email))
            {
                var users = await _userManager.Users.Select(u => new UserVM()
                {
                    Id = u.Id,
                    FName = u.FName,
                    LName = u.LName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result
                }).ToListAsync();
                return View(users);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);
                var userMapped = new UserVM()
                {
                    Id = user.Id,
                    FName = user.FName,
                    LName = user.LName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(user).Result
                };
                return View(new List<UserVM>() { userMapped });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();

            var userMapped = _mapper.Map<ApplicationUser, UserVM>(user);

            return View(viewName, userMapped);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // to work as route of browser
        public async Task<IActionResult> Edit([FromRoute] string? id, UserVM userVM)
        {
            if (id != userVM.Id)
                return BadRequest();

            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    //var userMapped = _mapper.Map<UserVM, ApplicationUser>(userVM); // deatached
                    var user = await _userManager.FindByIdAsync(id);
                    user.FName = userVM.FName;
                    user.LName = userVM.LName;
                    user.PhoneNumber = userVM.PhoneNumber;
                    //user.SecurityStamp=Guid.NewGuid().ToString(); // if i want to change email

                    await _userManager.UpdateAsync(user);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserVM userVM)
        {
            if (id != userVM.Id)
                return BadRequest();

            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);

                    await _userManager.DeleteAsync(user);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1) Log Exception
                    // 2) Friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(userVM);
        }
    }
}
