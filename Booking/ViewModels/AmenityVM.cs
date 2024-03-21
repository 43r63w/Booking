using Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Endpoint.ViewModels
{
    public class AmenityVM
    {
        public Amenity? Amenity  { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? VillasList { get; set; }
    }
}
