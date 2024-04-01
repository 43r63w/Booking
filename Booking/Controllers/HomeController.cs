
using Application.Common.Interfaces;
using Application.Utility;
using Booking.Models;
using Endpoint.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                VillasList = await _unitOfWork.IVillaService.GetAllAsync(includeOptions: "VillaAmenities"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            };



            return View(homeVM);
        }


        [HttpPost]
        public async Task<IActionResult> Index(HomeVM homeVM)
        {
            homeVM.VillasList = await _unitOfWork.IVillaService.GetAllAsync(includeOptions: "VillaAmenities");
            return View(homeVM);
        }

        public IActionResult GetVillasByDate(int nigths, DateOnly checkInDate)
        {
            Thread.Sleep(500);

            var villasList = _unitOfWork.IVillaService.GetAll(includeOptions: "VillaAmenities");

            var villasNumberList = _unitOfWork.IVillaRoomService.GetAll().ToList();

            var bookedVillas = _unitOfWork.IBookingService.GetAll(i => i.Status == SD.Approved ||
                                                                       i.Status == SD.CheckedIn).ToList();

            foreach (var villa in villasList)
            {
                int villasAvailable = SD.VillaRoomsAvailableCount(villa.Id, villasNumberList, checkInDate, nigths, bookedVillas);


                villa.IsAvailable = villasAvailable > 0 ? true : false;
            }

            HomeVM homeVM = new HomeVM
            {
                VillasList = villasList,
                Nights = nigths,
                CheckInDate = checkInDate,
            };

            return PartialView("_VillaList", homeVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
