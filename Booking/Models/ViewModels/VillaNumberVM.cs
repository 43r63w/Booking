﻿using Domain.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Endpoint.Models.ViewModels
{
    public class VillaNumberVM
    {
        public VillaNumber VillaNumber { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? VillaList { get; set; }
    }
}