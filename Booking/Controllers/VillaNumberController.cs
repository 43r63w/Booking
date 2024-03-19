using Application.Services;
using Domain.Models;
using Endpoint.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

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
            IEnumerable<VillaNumber> villaNumbers = await _unitOfWork.IVillaRoomService.GetAllAsync(includeOptions: "Villa");
            return View(villaNumbers);
        }
        [HttpGet]
        public async Task<IActionResult> Upsert(int villaId)
        {
            var fromDb = await _unitOfWork.IVillaRoomService.GetAsync(i => i.Villa_Number == villaId);

            if (fromDb == null)
            {
                VillaNumberVM villaNumberVM = new VillaNumberVM()
                {
                    VillaList = _unitOfWork.IVillaService.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                    VillaNumber = new VillaNumber()
                };
                return View(villaNumberVM);
            }

            var villaVm = new VillaNumberVM
            {

                VillaList = _unitOfWork.IVillaService.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                VillaNumber = fromDb
            };


            return View(villaVm);

        }

        [HttpPost]
        public async Task<IActionResult> Upsert(VillaNumberVM model)
        {
            var isExist = await _unitOfWork.IVillaRoomService.IsExistAsync(model.VillaNumber.Villa_Number);

            if (isExist)
            {
                TempData["error"] = "Rooms with this number is exist";
                return View(model);
            }

            if (ModelState.IsValid && !isExist)
            {
                _unitOfWork.IVillaRoomService.Add(model.VillaNumber);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Villa Create/Update";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Something error,try again";

            model.VillaList = _unitOfWork.IVillaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Remove(int villaId)
        {
            var fromDb = await _unitOfWork.IVillaService.GetAsync(i => i.Id == villaId);

            return View(fromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(VillaNumber item)
        {
            var fromDb = await _unitOfWork.IVillaRoomService.GetAsync(i => i.Villa_Number == item.Villa_Number);
            _unitOfWork.IVillaRoomService.Remove(fromDb);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Villa Remove";
            return RedirectToAction("Index");
        }

    }
}
