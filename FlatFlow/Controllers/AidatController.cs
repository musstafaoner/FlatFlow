using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlatFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Yonetici")] // Sadece yöneticiler aidat atayabilir
    public class AidatController : Controller
    {
        private readonly AppDbContext _context;

        public AidatController(AppDbContext context)
        {
            _context = context;
        }

        // 1. AİDATLARI LİSTELEME EKRANI
        public async Task<IActionResult> Index()
        {
            // Aidatları, ait oldukları Daire bilgisiyle beraber getiriyoruz
            var aidatlar = await _context.Aidatlar.Include(a => a.Daire).ToListAsync();
            return View(aidatlar);
        }

        // 2. YENİ AİDAT EKLEME EKRANI (GET)
        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            // Hangi daireye aidat yazılacağını seçmek için daireleri gönderiyoruz
            ViewBag.Daireler = await _context.Daireler.ToListAsync();
            return View();
        }

        // 3. YENİ AİDAT EKLEME İŞLEMİ (POST)
        [HttpPost]
        public async Task<IActionResult> Ekle(Aidat model)
        {
            // Yeni eklenen aidat varsayılan olarak "Ödenmedi" (false) durumundadır
            model.OdendiMi = false;

            _context.Aidatlar.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}