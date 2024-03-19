using Application.Services;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EndpointUI.Controllers
{

    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Villa> villas = await _unitOfWork.IVillaService.GetAllAsync();
            return View(villas);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(int villaId)
        {
            var fromDb = await _unitOfWork.IVillaService.GetAsync(i => i.Id == villaId);

            if (fromDb == null)
            {
                return View(new Villa());
            }   
            return View(fromDb);

        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Villa model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.IVillaService.Add(model);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Villa Create/Update";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Something error,try again";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int villaId)
        {
            var fromDb = await _unitOfWork.IVillaService.GetAsync(i => i.Id == villaId);
        
            return View(fromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Villa item)
        {
            var fromDb = await _unitOfWork.IVillaService.GetAsync(i => i.Id == item.Id);
            _unitOfWork.IVillaService.Remove(fromDb);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Villa Remove";
            return RedirectToAction("Index");
        }
    }




}
