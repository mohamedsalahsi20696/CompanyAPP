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
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        public RoleController(RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                var roles = await _roleManager.Roles.Select(r => new RoleVM()
                {
                    Id = r.Id,
                    RoleName = r.Name,
                }).ToListAsync();
                return View(roles);
            }
            else
            {
                var roles = await _roleManager.FindByNameAsync(name);
                var roleMapped = new RoleVM()
                {
                    Id = roles.Id,
                    RoleName = roles.Name,
                };
                return View(new List<RoleVM>() { roleMapped });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleVM roleVM)
        {

            if (ModelState.IsValid) // server side validation
            {
                var roleMapped = _mapper.Map<RoleVM, IdentityRole>(roleVM);
                await _roleManager.CreateAsync(roleMapped);
                
                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            var roleMapped = _mapper.Map<IdentityRole, RoleVM>(role);

            return View(viewName, roleMapped);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // to work as route of browser
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleVM roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();

            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = roleVM.RoleName;
                    
                    await _roleManager.UpdateAsync(role);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleVM roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();

            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var user = await _roleManager.FindByIdAsync(id);

                    await _roleManager.DeleteAsync(user);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1) Log Exception
                    // 2) Friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleVM);
        }
    }
}
