using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Repositories;
using CompanyAPP.Helpers;
using CompanyAPP.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyAPP.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // to work dependancy injection should add this line in startup class
        // services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string searchVal)
        {
            //1) viewData => keyValuePair[Dicionary object]
            // transfer data from action to view
            // .net framework 3.5
            ViewData["Message"] = "Hello From ViewData";

            //2) viewData => dynamic property
            // transfer data from action to view
            // .net framework 4.0
            ViewBag.msg = "Hello From ViewBag";

            var emps = await _unitOfWork.EmployeeRepository.GetAll();
            // transform from model to view model

            if (!string.IsNullOrEmpty(searchVal))
                emps = emps.Where(e => e.Name.ToLower().Contains(searchVal.ToLower()));

            var empMappeds = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeVM>>(emps);

            return View(empMappeds);
        }


        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.departments = _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(EmployeeVM empVM)
        {

            if (ModelState.IsValid) // server side validation
            {
                // transform from View model to model
                var empMapped = _mapper.Map<EmployeeVM, Employee>(empVM);
                empMapped.ImageName = DocumentSettings.uploadImage(empVM.Image, "Images");
                await _unitOfWork.EmployeeRepository.Add(empMapped);
                int res = await _unitOfWork.Complete();
                if (res > 0)
                {
                    //3) tempData => dicitionart object
                    // transfer data from action to action
                    TempData["Message"] = "Employee is Created";
                }
                return RedirectToAction(nameof(Index));
            }

            return View(empVM);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var emp = await _unitOfWork.EmployeeRepository.GetById(id.Value);

            if (emp is null)
                return NotFound();

            var empMapped = _mapper.Map<Employee, EmployeeVM>(emp);

            return View(viewName, empMapped);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.departments = await _unitOfWork.DepartmentRepository.GetAll();
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  // to work as route of browser
        public async Task<IActionResult> Edit([FromRoute] int? id, EmployeeVM empVM)
        {
            if (id != empVM.Id)
                return BadRequest();

            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var empMapped = _mapper.Map<EmployeeVM, Employee>(empVM);
                    _unitOfWork.EmployeeRepository.Update(empMapped);
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

            return View(empVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeVM empVM)
        {
            if (id != empVM.Id)
                return BadRequest();

            if (ModelState.IsValid) // server side validation
            {
                try
                {
                    var empMapped = _mapper.Map<EmployeeVM, Employee>(empVM);

                    if (empMapped.ImageName is not null)
                    {
                        DocumentSettings.DeleteImage(empMapped.ImageName, "Images");
                    }
                    _unitOfWork.EmployeeRepository.Delete(empMapped);
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
            return View(empVM);
        }
    }
}
