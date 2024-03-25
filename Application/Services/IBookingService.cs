using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{

    public interface IBookingService : IRepository<Domain.Models.Booking>
    {
        void UpdateStatus(int bookingId, string status);
        void UpdateStripePaymentId(int bookingId, string sessionId, string paymentIntentId);
    }

}
