using Application.Services;
using Domain.Models;
using Endpoint.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Endpoint.Controllers
{
    public class VillaAmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaAmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Upsert(int amenityId)
        {
            AmenityVM amenityVM = new AmenityVM
            {
                Amenity = await _unitOfWork.IAmenityService.GetAsync(i => i.Id == amenityId) ?? new Amenity(),

                VillasList = _unitOfWork.IVillaService.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            return View(amenityVM);

        }

        [HttpPost]
        public async Task<IActionResult> Upsert(AmenityVM model)
        {

            if (ModelState.IsValid)
            {
                if (model.Amenity.Id == 0)
                    _unitOfWork.IAmenityService.Add(model.Amenity);
                else
                    _unitOfWork.IAmenityService.Update(model.Amenity);

                await _unitOfWork.SaveAsync();
                TempData["success"] = "Amenity Create/Update";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Something error,try again";

            AmenityVM amenityVM = new AmenityVM
            {
                Amenity = new Amenity(),

                VillasList = _unitOfWork.IVillaService.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };


            return View(amenityVM);
        }


        #region DataTableCall
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenities = await _unitOfWork.IAmenityService.GetAllAsync(includeOptions:"Villa");

            return Json(new { data = amenities });
        }
        [HttpDelete]
        public async Task<IActionResult> Remove(int amenityId)
        {
            var fromDb = await _unitOfWork.IAmenityService.GetAsync(e => e.Id == amenityId);

            _unitOfWork.IAmenityService.Remove(fromDb);
            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = "Amenity Remove" });
        }

        #endregion





    }
}
