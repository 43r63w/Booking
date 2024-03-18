using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EndpointUI.Controllers
{

    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Villa> villasList = await _context.Villas.AsNoTracking().ToListAsync();

            return View(villasList);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Villa model)
        {
            _context.Villas.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int Id)
        {
            _context.Remove(Id);
            await _context.SaveChangesAsync();  
           
        
            return Ok();
        }



        [HttpPatch]
        public async Task<IActionResult> Upsert1(Villa model)
        {
            return View();
        }

    }




}
