using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlatFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Yonetici")] // Burası sadece Yöneticilerin mekanı
    public class ArizaController : Controller
    {
        private readonly AppDbContext _context;

        public ArizaController(AppDbContext context)
        {
            _context = context;
        }

        // SİSTEMDEKİ TÜM ARIZALARI LİSTELE
        public async Task<IActionResult> Index()
        {
            var arizalar = await _context.ArizaTalepleri
                .Include(a => a.Kullanici)
                .OrderByDescending(a => a.OlusturulmaTarihi)
                .ToListAsync();

            return View(arizalar);
        }

        // ARIZA DURUMUNU GÜNCELLEME (POST)
        [HttpPost]
        public async Task<IActionResult> DurumGuncelle(int id, string yeniDurum)
        {
            var ariza = await _context.ArizaTalepleri.FindAsync(id);
            if (ariza != null)
            {
                ariza.Durum = yeniDurum;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}