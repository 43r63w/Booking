
using Application.Common.Interfaces;
using Application.Utility;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Net.Http.Headers;

namespace EndpointUI.Controllers
{

    [Authorize(Roles = SD.Admin)]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public VillaController(IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var token = HttpContext.Request.Cookies["Token"];

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
            var directoryName = model.Name;

            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                    string finalPath = Path.Combine(_webHostEnvironment.WebRootPath + @$"\Images\Villa\{directoryName}");

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }

                    if (System.IO.File.Exists(model.ImageUrl))
                    {
                        System.IO.File.Delete(_webHostEnvironment.WebRootPath + @"\" + model.ImageUrl);
                    }

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        model.Image.CopyTo(fileStream);
                    }

                    model.ImageUrl = @$"\Images\Villa\{directoryName}\" + fileName;
                }
                else
                {
                    model.ImageUrl = "https://placehold.co/600x400";
                }


                if (model.Id == 0)
                    _unitOfWork.IVillaService.Add(model);
                else
                    _unitOfWork.IVillaService.Update(model);

                await _unitOfWork.SaveAsync();
                TempData["success"] = "Villa Create/Update";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Something error,try again";
            return View(model);
        }


        #region DataTableCall 
        [HttpGet]
        public async Task<IActionResult> GetAll() => Json(new { data = await _unitOfWork.IVillaService.GetAllAsync() });

        [HttpDelete]
        public async Task<IActionResult> Remove(int villaId)
        {
            var fromDb = await _unitOfWork.IVillaService.GetAsync(e => e.Id == villaId);
            var directoryPath = _webHostEnvironment.WebRootPath + Path.GetDirectoryName(fromDb.ImageUrl);
            System.IO.Directory.Delete(directoryPath, true);


            _unitOfWork.IVillaService.Remove(fromDb);
            await _unitOfWork.SaveAsync();
            return Json(new { success = true, message = "Villa Deleted" });
        }

        #endregion
    }




}
