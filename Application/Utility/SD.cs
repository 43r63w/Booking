using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utility
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";


        public const string Completed = "Completed";

        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Cancelled = "Cancelled";
        public const string Refunded = "Refunded";
        public const string CheckedIn = "CheckedIn";

        public static int VillaRoomsAvailableCount(int villaId,
            List<VillaNumber> villaNumberList,
            DateOnly checkInDate,
            int nights,
            List<Booking> bookings)
        {
            List<int> bookingDate = new();

            int finalAvailableRoomForAllNights = int.MaxValue;

            var villasCount = villaNumberList.Where(i => i.VillaId == villaId).Count();

            for (int i = 0; i < nights; i++)
            {
                var villasBooked = bookings.Where(e => e.CheckInDate <= checkInDate.AddDays(i) &&
                                                      e.CheckOutDate >= checkInDate.AddDays(i) && e.VillaId == villaId);
                foreach (var booking in villasBooked)
                {
                    if (!bookingDate.Contains(booking.Id))
                    {
                        bookingDate.Add(booking.Id);
                    }
                }

                var totalAvailableRooms = villasCount - bookingDate.Count();

                if (totalAvailableRooms == 0)
                {
                    return 0;
                }
                else
                {
                    if (finalAvailableRoomForAllNights > totalAvailableRooms)
                    {
                        finalAvailableRoomForAllNights = totalAvailableRooms;
                    }
                }

            }
            return finalAvailableRoomForAllNights;

        }

    }
}
