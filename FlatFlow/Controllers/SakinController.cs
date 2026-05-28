using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlatFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Sakin")]
    public class SakinController : Controller
    {
        private readonly AppDbContext _context;

        public SakinController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Borclarim()
        {
            var kullaniciIdUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (kullaniciIdUserId == null) return RedirectToAction("GirisYap", "Hesap");

            int id = int.Parse(kullaniciIdUserId);

            var borclar = await _context.Aidatlar
                .Include(a => a.Daire)
                .Where(a => a.Daire.KullaniciId == id)
                .ToListAsync();

            return View(borclar);
        }

        [HttpPost]
        public async Task<IActionResult> OdemeYap(int aidatId)
        {
            var aidat = await _context.Aidatlar.FindAsync(aidatId);
            var kullaniciIdUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (aidat != null && kullaniciIdUserId != null)
            {
                aidat.OdendiMi = true;

                var yeniOdeme = new Odeme
                {
                    AidatId = aidat.AidatId,
                    KullaniciId = int.Parse(kullaniciIdUserId),
                    OdenenTutar = aidat.Tutar,
                    OdemeTarihi = System.DateTime.Now
                };

                _context.Odemeler.Add(yeniOdeme);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Borclarim");
        }

        public async Task<IActionResult> Arizalarim()
        {
            var kullaniciId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var arizalar = await _context.ArizaTalepleri
                .Where(a => a.KullaniciId == int.Parse(kullaniciId))
                .OrderByDescending(a => a.OlusturulmaTarihi)
                .ToListAsync();

            return View(arizalar);
        }

        [HttpGet]
        public IActionResult ArizaBildir()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ArizaBildir(string Baslik, string Aciklama)
        {
            var kullaniciIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(kullaniciIdStr))
            {
                return RedirectToAction("GirisYap", "Hesap");
            }

            int kullaniciId = int.Parse(kullaniciIdStr);

            var daire = await _context.Daireler.FirstOrDefaultAsync(d => d.KullaniciId == kullaniciId);

            if (daire == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var yeniAriza = new ArizaTalep
            {
                Baslik = Baslik,
                Aciklama = Aciklama,
                Durum = "Beklemede",
                OlusturulmaTarihi = DateTime.Now,
                KullaniciId = kullaniciId,
                SiteId = daire.SiteId 
            };

            _context.ArizaTalepleri.Add(yeniAriza);
            await _context.SaveChangesAsync();

            return RedirectToAction("Arizalarim");
        }
    }
}