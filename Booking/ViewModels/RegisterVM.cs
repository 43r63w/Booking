using Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Endpoint.ViewModels
{
    public class RegisterVM
    {
        public User? User { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? RolesList { get; set; }
    }
}
