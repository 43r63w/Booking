using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;

        public string SurName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]    
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [NotMapped]
        //  [ValidateNever]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } = string.Empty;
        [NotMapped]
        public bool IsAuthentication { get; set; }  

    }
}
