using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
