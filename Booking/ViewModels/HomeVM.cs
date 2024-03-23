using Domain.Models;

namespace Endpoint.ViewModels
{
    public class HomeVM
    {   
        public IEnumerable<Villa>? VillasList { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
