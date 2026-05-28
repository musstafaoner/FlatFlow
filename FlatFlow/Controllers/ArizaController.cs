using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlatFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Yonetici")] 
    public class ArizaController : Controller
    {
        private readonly AppDbContext _context;

        public ArizaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");

            if (aktifSiteId == null || aktifSiteId == 0)
            {
                return RedirectToAction("Index", "Site");
            }

            var arizalar = await _context.ArizaTalepleri
                .Include(a => a.Kullanici)
                .Where(a => a.SiteId == aktifSiteId)
                .OrderByDescending(a => a.OlusturulmaTarihi)
                .ToListAsync();

            var kullaniciIdleri = arizalar.Select(a => a.KullaniciId).Distinct().ToList();

            var daireSözlügü = await _context.Daireler
                .Where(d => d.SiteId == aktifSiteId && d.KullaniciId != null && kullaniciIdleri.Contains(d.KullaniciId.Value))
                .ToDictionaryAsync(d => d.KullaniciId.Value, d => d);

            ViewBag.Daireler = daireSözlügü;

            return View(arizalar);
        }

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