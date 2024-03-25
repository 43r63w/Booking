
using Application.Services;
using Application.Utility;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            IUnitOfWork unitOfWork,
           UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public async Task<IActionResult> Register(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (!_roleManager.RoleExistsAsync(SD.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }

            var registerVM = new RegisterVM
            {
                RolesList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                })
            };

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVmModel)
        {
            var newUser = new ApplicationUser
            {
                Email = registerVmModel.Email,
                EmailConfirmed = true,
                Name = registerVmModel.Name,
                PhoneNumber = registerVmModel.PhoneNumber,
                NormalizedEmail = registerVmModel.Email.ToUpper(),
                UserName = registerVmModel.Email
            };

            var result = _userManager.CreateAsync(newUser, registerVmModel.Password).GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(registerVmModel.Role))
                {
                    _userManager.AddToRoleAsync(newUser, registerVmModel.Role!).GetAwaiter().GetResult();
                }
                else
                {
                    _userManager.AddToRoleAsync(newUser, SD.Customer).GetAwaiter().GetResult();
                }
                _signInManager.SignInAsync(newUser, isPersistent: false).GetAwaiter().GetResult();

                if (string.IsNullOrEmpty(registerVmModel.RedirectUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(registerVmModel.RedirectUrl);
                }

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            registerVmModel.RolesList = _roleManager.Roles.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Name
            });

            return View(registerVmModel);

        }


        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var loginVm = new LoginVM
            {
                RedirectUrl = returnUrl,
            };

            return View(loginVm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                        return RedirectToAction("Index", "Home");
                    else
                        return LocalRedirect(loginVM.RedirectUrl);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something wrong,try again");
            }

            return View(loginVM);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }



    }
}
