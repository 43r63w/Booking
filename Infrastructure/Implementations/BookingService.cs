using Application.Common.Interfaces;
using Application.Utility;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations
{
    public class BookingService : Repository<Domain.Models.Booking>, IBookingService
    {

        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Domain.Models.Booking> GetBookings(string? status)
        {
            if (status == "null" || status == "All")
                return _context.Bookings.ToList();


            return _context.Bookings.Where(u => u.Status == status).ToList();


        }

        public void UpdateStatus(int bookingId, string updateStatus, int villaNumber = 0)
        {
            var booking = _context.Bookings.FirstOrDefault(i => i.Id == bookingId);
            if (booking != null)
            {
                booking.Status = updateStatus;

                if (updateStatus == SD.CheckedIn)
                {
                    booking.VillaNumber = villaNumber;
                    booking.ActualCheckInDate = DateTime.Now;
                }


                if (updateStatus == SD.Approved)
                    booking.ActualCheckOutDate = DateTime.Now;
            }

        }


        public void UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntentId)
        {
            var booking = _context.Bookings.FirstOrDefault(i => i.Id == bookingId);

            if (booking != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                    booking.StripeSessionId = sessionId;

                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    booking.StripePaymentIntentId = paymentIntentId;
                    booking.PaymentDate = DateTime.Now;
                    booking.IsPaymentSuccessfully = true;
                }

            }
        }

    }


}

