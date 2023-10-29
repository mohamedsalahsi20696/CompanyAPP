using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Repositories;
using CompanyAPP.ViewModels;
using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPP.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // to work dependancy injection should add this line in startup class
        // services.AddScoped<IDepartmentRepository,DepartmentRepository>();
        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchVal)
        {
            var depts = await _unitOfWork.DepartmentRepository.GetAll();

            if (!string.IsNullOrEmpty(searchVal))
                depts = depts.Where(e => e.Name.ToLower().Contains(searchVal.ToLower()));


            var deptMappeds = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentVM>>(depts);

            return View(deptMappeds);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentVM deptVM)
        {
            if (ModelState.IsValid) // server side validation
            {
                var deptMapped = _mapper.Map<DepartmentVM, Department>(deptVM);

                await _unitOfWork.DepartmentRepository.Add(deptMapped);
                await _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }

            return View(deptVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var dept = await _unitOfWork.DepartmentRepository.GetById(id.Value);

            if (dept is null)
                return NotFound();

            var deptMapped = _mapper.Map<Department, DepartmentVM>(dept);


            return View(viewName, deptMapped);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // to work as route of browser
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentVM departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var deptMapped = _mapper.Map<DepartmentVM, Department>(departmentVM);

                    _unitOfWork.DepartmentRepository.Update(deptMapped);
                    await _unitOfWork.Complete();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1) Log Exception
                    // 2) Friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(departmentVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // to work as route of browser
        public async Task<IActionResult> Delete([FromRoute] int? id, DepartmentVM departmentVM)
        {
            if (id != departmentVM.Id)
                return BadRequest();

            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var deptMapped = _mapper.Map<DepartmentVM, Department>(departmentVM);

                    _unitOfWork.DepartmentRepository.Delete(deptMapped);
                    await _unitOfWork.Complete();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1) Log Exception
                    // 2) Friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }
    }
}
