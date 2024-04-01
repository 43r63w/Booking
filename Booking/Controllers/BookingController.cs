using Application.Common.Interfaces;
using Application.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;
using Stripe.Checkout;
using System.Net.Http.Headers;
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

        public IActionResult Index(string status)
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


            var domain = Request.Scheme + "://" + Request.Host.Value + "/";

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Booking/BookingConfirmation?bookingId={booking.Id}",
                CancelUrl = domain + $"Booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
            };



            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalCost * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villa.Name,
                        Description = villa.Description,
                    }

                },
                Quantity = 1
            });



            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);


            _unitOfWork.IBookingService.UpdateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);
            await _unitOfWork.SaveAsync();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [Authorize]
        public async Task<IActionResult> BookingConfirmation(int bookingId)
        {

            var getBooking = await _unitOfWork.IBookingService.GetAsync(i => i.Id == bookingId, includeOptions: "Villa,User");

            if (getBooking.Status == SD.Pending)
            {
                var service = new SessionService();
                Session session = service.Get(getBooking.StripeSessionId);

                if (session.PaymentStatus == "paid")
                {
                    _unitOfWork.IBookingService.UpdateStatus(getBooking.Id, SD.Approved);
                    _unitOfWork.IBookingService.UpdateStripePaymentId(getBooking.Id, session.Id, session.PaymentIntentId);
                    _unitOfWork.SaveAsync();
                }

            }


            return View(bookingId);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBooking(int bookingId)
        {
            var booking = await _unitOfWork.IBookingService.GetAsync(i => i.Id == bookingId, includeOptions: "Villa,User");

            if (booking.VillaNumber == 0
                && booking.Status == SD.Approved)
            {
                var availableRooms = GetAvailableVillaNumbers(booking.VillaId);

                booking.VillasNumber = _unitOfWork.IVillaRoomService.GetAll(i => i.VillaId == booking.VillaId &&
                                                                          availableRooms.Any(u => u == i.Villa_Number)).ToList();
            }

            if (booking != null)
            {
                return View(booking);
            }

            return View();
        }


        [HttpPost]
        [Authorize(Roles = SD.Admin)]
        public async Task<IActionResult> CheckIn(Domain.Models.Booking booking)
        {


            _unitOfWork.IBookingService.UpdateStatus(booking.Id, SD.CheckedIn, booking.VillaNumber);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Booking successfully update";

            return RedirectToAction(nameof(GetBooking), new { bookingId = booking.Id });
        }


        [HttpPost]
        [Authorize(Roles = SD.Admin)]
        public async Task<IActionResult> CheckOut(Domain.Models.Booking booking)
        {
            _unitOfWork.IBookingService.UpdateStatus(booking.Id, SD.Completed, booking.VillaNumber);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Booking successfully completed";

            return RedirectToAction(nameof(GetBooking), new { bookingId = booking.Id });
        }


        [HttpPost]
        [Authorize(Roles = SD.Admin)]
        public async Task<IActionResult> Cancel(Domain.Models.Booking booking)
        {
            _unitOfWork.IBookingService.UpdateStatus(booking.Id, SD.Cancelled, 0);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Booking successfully update";

            return RedirectToAction(nameof(GetBooking), new { bookingId = booking.Id });
        }



        #region DataTablesCall
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(string status)
        {
         
            IEnumerable<Domain.Models.Booking>? getBooking;

            if (User.IsInRole(SD.Admin))
            {
                getBooking = _unitOfWork.IBookingService.GetBookings(status);
                return Json(new { data = getBooking });
            }

            getBooking = _unitOfWork.IBookingService.GetBookings(status);

            return Json(new { data = getBooking });

        }

        private List<int> GetAvailableVillaNumbers(int villaId)
        {
            List<int> availableVillaRooms = new();

            var villasNumber = _unitOfWork.IVillaRoomService.GetAll(i => i.VillaId == villaId);

            var checkedInVilla = _unitOfWork.IBookingService.GetAll(i => i.VillaId == villaId && i.Status == SD.Completed && i.Status == SD.CheckedIn)
                .Select(i => i.VillaNumber);

            foreach (var villa in villasNumber)
            {
                if (!checkedInVilla.Contains(villa.Villa_Number))
                {
                    availableVillaRooms.Add(villa.Villa_Number);
                }
            }

            return availableVillaRooms;
        }


        #endregion




    }

}
