using Application.JWT;
using Application.Services;
using Domain;
using Domain.Models;
using Endpoint.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Composition.Convention;
using System.Diagnostics;

namespace Endpoint.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTService _jWTService;

        private static List<string> _roles = [SD.Admin, SD.Customer];

        public AccountController(
            IUnitOfWork unitOfWork,
            JWTService jWTService)
        {
            _unitOfWork = unitOfWork;
            _jWTService = jWTService;
        }



        [HttpGet]
        public IActionResult Register()
        {
            RegisterVM registerVM = new RegisterVM
            {
                User = new User(),

                RolesList = _roles.Select(r => new SelectListItem
                {
                    Text = r.ToString(),
                    Value = r
                })
            };

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVmModel)
        {

            if (ModelState.IsValid)
            {
                var result = await _unitOfWork.IUserService.Register(registerVmModel.User);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "Account successfully created";
                    return RedirectToAction("Index", "Home");
                }

            }

            RegisterVM registerVM = new RegisterVM
            {
                RolesList = _roles.Select(r => new SelectListItem
                {
                    Text = r.ToString(),
                    Value = r
                })
            };
            return View(registerVM);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var token = await _unitOfWork.IUserService.Login(loginVM.User);

         
                

            HttpContext.Response.Cookies.Append("Token", token);

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Cookies["Token"];
            if (token != null)
            {
                HttpContext.Response.Cookies.Delete("Token");            
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction();     
        }



    }
}
