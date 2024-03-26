using Application.Services;
using Application.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Stripe;
using Stripe.Checkout;
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
        #region DataTablesCall
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var claim = (ClaimsIdentity)User.Identity;

            var userRole = claim.FindFirst(ClaimTypes.Role).Value;

            if (userRole == SD.Admin)
            {
                var bookings = await _unitOfWork.IBookingService.GetAllAsync(includeOptions: "Villa,User");
                return Json(new { data = bookings });
            }
            else if (userRole == SD.Customer)
            {
                var userId = claim.FindFirst(ClaimTypes.NameIdentifier).Value;
                var bookings = await _unitOfWork.IBookingService.GetAllAsync(i => i.UserId == userId, includeOptions: "Villa,User");
                return Json(new { data = bookings });
            }

            throw new ApplicationException();
        }

        #endregion


        #region Helper Method
        [Authorize]
        private async Task<IEnumerable<Domain.Models.Booking>> ShowBookingsWithParameters(string status)
        {
            return await _unitOfWork.IBookingService.GetAllAsync(e => e.Status == status);
        }
        #endregion


    }











}
