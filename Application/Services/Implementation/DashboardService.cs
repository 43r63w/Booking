using Application.Common.Interfaces;
using Application.Utility;
using Endpoint.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;

        private DateTime previousStartMonth = new(DateTime.Now.Year, previousMonth, 1);
        private DateTime currentStartMonth = new(DateTime.Now.Year, DateTime.Now.Month, 1);

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<LineChartDTO> GetMemberAndLineChartData()
        {
            var bookingData = _unitOfWork.IBookingService.GetAll(i => i.BookingDate >= DateTime.Now.AddDays(-30)
           && i.BookingDate <= DateTime.Now)
               .GroupBy(x => x.BookingDate.Date)
               .Select(u => new
               {
                   DateTime = u.Key,
                   NewBookingCount = u.Count(),
               });


            var customerData = _unitOfWork.IUserService.GetAll(u => u.TimeToCreateAccount >= DateTime.Now.AddDays(-30) &&
                u.TimeToCreateAccount <= DateTime.Now).
                   GroupBy(x => x.TimeToCreateAccount.Date)
                      .Select(i => new
                      {
                          DateTime = i.Key,
                          NewUserCount = i.Count(),
                      });


            var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime,
              customer => customer.DateTime,
              (booking, customer) => new
              {
                  booking.DateTime,
                  booking.NewBookingCount,
                  NewUserCount = customer.Select(x => x.NewUserCount).FirstOrDefault()
              });

            var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime,
            booking => booking.DateTime,
            (customer, booking) => new
            {
                customer.DateTime,
                NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
                customer.NewUserCount

            });



            var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();



            var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
            var newUserData = mergedData.Select(x => x.NewUserCount).ToArray();

            var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();


            List<ChartDate> chartDates = new()
            {
               new ChartDate
               {
                   Name = "New Bookings",
                   Data = newBookingData
               },

                 new ChartDate
                 {
                   Name = "New Users",
                   Data = newUserData
                 }
            };

            LineChartDTO LineChartDTO = new()
            {
                Categories = categories,
                Series = chartDates
            };



            return LineChartDTO;
        }

        public async Task<PieChartDTO> GetTotalBookingsPieChart()
        {
            var totalBooking = await _unitOfWork.IBookingService.GetAllAsync(i => i.BookingDate >= DateTime.Now.AddDays(-30) &&
                                                                            (i.Status != SD.Pending || i.Status == SD.Cancelled));

            var customerWithOneBooking = totalBooking.GroupBy(i => i.UserId).Where(u => u.Count() == 1).Select(x => x.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();

            int bookingsReturningCustomer = totalBooking.Count() - bookingsByNewCustomer;


            var PieChartDTO = new PieChartDTO
            {
                Labels = new string[] { "New customer", "Returning Customer Bookings" },
                Series = new int[] { bookingsByNewCustomer, bookingsReturningCustomer }
            };

            return PieChartDTO;
        }

        public async Task<RadialChartBarDTO> GetTotalBookingsRadialChart()
        {
            var totalBooking = await _unitOfWork.IBookingService.GetAllAsync(i => i.Status != SD.Pending
                                                                           || i.Status == SD.Cancelled);

            var countByCurrentMonth = totalBooking.Count(i => i.BookingDate >= currentStartMonth
                                                           && i.BookingDate <= DateTime.Now);

            var countByPreviousMonth = totalBooking.Count(i => i.BookingDate >= previousStartMonth && i.BookingDate <= currentStartMonth);


            return SetPropertyForRadialChart(totalBooking.Count(), countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RadialChartBarDTO> GetTotalRevenueRadialChart()
        {


            var totalBooking = await _unitOfWork.IBookingService.GetAllAsync(i => i.Status != SD.Pending
                                                                           || i.Status == SD.Cancelled);

            var totalRevenue = Convert.ToInt32(totalBooking.Sum(i => i.TotalCost));

            var revenueCurrentMonth = Convert.ToInt32(totalBooking.Where(i => i.BookingDate >= currentStartMonth
                                                            && i.BookingDate <= DateTime.Now).Sum(i => i.TotalCost));

            var revenuePreviousMonth = Convert.ToInt32(totalBooking.Where(i => i.BookingDate >= previousStartMonth
                                                            && i.BookingDate <= currentStartMonth).Sum(i => i.TotalCost));

            return SetPropertyForRadialChart(totalRevenue, revenueCurrentMonth, revenuePreviousMonth);
        }

        public async Task<RadialChartBarDTO> GetTotalUserRadialChart()
        {
            var totalUsers = await _unitOfWork.IUserService.GetAllAsync();

            int currentMonthRegisterUsers = totalUsers.Count(i => i.TimeToCreateAccount >=
                                                            currentStartMonth && i.TimeToCreateAccount <= DateTime.Now);

            int previousMonthRegisterUsers = totalUsers.Count(i => i.TimeToCreateAccount >= previousStartMonth && i.TimeToCreateAccount
                                                                        <= currentStartMonth);

            return SetPropertyForRadialChart(totalUsers.Count(), currentMonthRegisterUsers, previousMonthRegisterUsers);
        }


        #region HelperMethod
        private RadialChartBarDTO SetPropertyForRadialChart(int totalCount, int countByCurrentMonth, int previousByMonth)
        {
            RadialChartBarDTO RadialChartBarDTO = new();

            int increaseDeacreaseRation = 100;

            if (previousByMonth != 0)
            {
                increaseDeacreaseRation = Convert.ToInt32((countByCurrentMonth - previousByMonth) / previousByMonth * 100);
            }

            return new RadialChartBarDTO
            {
                TotalCount = totalCount,
                CountByCurrentMonth = countByCurrentMonth,
                Series = new int[] { increaseDeacreaseRation },
                HasRatioIncreased = countByCurrentMonth > previousByMonth,
            };


        }
        #endregion

    }
}
