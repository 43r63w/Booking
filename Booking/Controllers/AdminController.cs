using Application.Common.Interfaces;
using Application.Services;
using Application.Utility;
using Endpoint.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Stripe;

namespace Endpoint.Controllers
{

    // [Authorize(Roles = SD.Admin)]
    public class AdminController : Controller
    {
        private readonly IDashboardService _dashboardService;

        private static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        private DateTime previousStartMonth = new(DateTime.Now.Year, previousMonth, 1);
        private DateTime currentStartMonth = new(DateTime.Now.Year, DateTime.Now.Month, 1);


        public AdminController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingsRadialChart()
        {
            return Json(await _dashboardService.GetTotalBookingsRadialChart());
        }

        public async Task<IActionResult> GetTotalUserRadialChart()
        {
            return Json(await _dashboardService.GetTotalUserRadialChart());
        }
        public async Task<IActionResult> GetTotalRevenueChart()
        {
           return Json(await _dashboardService.GetTotalRevenueRadialChart());

        }


        public async Task<IActionResult> GetTotalBookingsPieChart()
        {
            return Json(await _dashboardService.GetTotalBookingsPieChart());
        }


        public async Task<IActionResult> GetMemberAndLineChartData()
        {

            return Json(await _dashboardService.GetMemberAndLineChartData());
        }




    }
}
