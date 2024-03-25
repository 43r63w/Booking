using Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Endpoint.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LastName { get; set; }
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string? Role { get; set; }



        [ValidateNever]
        public IEnumerable<SelectListItem>? RolesList { get; set; }

        public string? RedirectUrl { get; set; }
    }
}
