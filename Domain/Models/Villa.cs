using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Villa
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Range(1, 10000)]
        public double Price { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public double Sqft { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [Range(1, 30)]
        public int Occupancy { get; set; }
        [NotMapped]
        public  IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }


        [ValidateNever]
        [JsonIgnore]
        public IEnumerable<Amenity> VillaAmenities { get; set; }


        [NotMapped]
        public bool IsAvailable { get; set; } = true;
    }

}
