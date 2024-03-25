using Application.Services;
using Application.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Endpoint.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
        {
            Domain.Models.Booking booking = new Domain.Models.Booking
            {
                VillaId = villaId,
                Nights = nights,
                CheckInDate = checkInDate,
                Villa = await _unitOfWork.IVillaService.GetAsync(i => i.Id == villaId, includeOptions: "VillaAmenities"),
                CheckOutDate = checkInDate.AddDays(nights),
            };

            return View(booking);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> FinalizeBooking(Domain.Models.Booking booking)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;      
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;  

            var villa = await _unitOfWork.IVillaService.GetAsync(i => i.Id == booking.VillaId);
            booking.TotalCost = villa.Price * booking.Nights;


            booking.Status = SD.Pending;
            booking.BookingDate = DateTime.Now;
            booking.UserId = userId;

            _unitOfWork.IBookingService.Add(booking);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(BookingConfirmation), new { bookingId =  booking.Id});

        }


        [Authorize]
        public async Task<IActionResult> BookingConfirmation(int bookingId)
        {
            return View(bookingId);
        }



    }
}
