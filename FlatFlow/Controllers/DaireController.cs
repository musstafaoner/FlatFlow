using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlatFlow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Yonetici")]
    public class DaireController : Controller
    {
        private readonly AppDbContext _context;

        public DaireController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var daireler = await _context.Daireler.Include(d => d.Kullanici).ToListAsync();
            return View(daireler);
        }

        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            var sakinler = await _context.Kullanicilar
                                         .Include(k => k.Rol)
                                         .Where(k => k.Rol.Ad == "Sakin")
                                         .ToListAsync();

            ViewBag.Sakinler = sakinler;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Daire model)
        {
            model.BosMu = model.KullaniciId == null;
            _context.Daireler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}