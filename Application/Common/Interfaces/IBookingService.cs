using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{

    public interface IBookingService : IRepository<Booking>
    {
        void UpdateStatus(int bookingId, string status, int villaNumber = 0);
        void UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntentId);

        IEnumerable<Booking> GetBookings(string? status);
    }

}
