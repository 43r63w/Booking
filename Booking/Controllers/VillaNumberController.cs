using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Controllers
{
    public class VillaNumberController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<VillaNumber> villaNumbers = await _unitOfWork.IVillaRoomService.GetAllAsync(includeOptions:"Villa");

            return View(villaNumbers);
        }
    }
}
