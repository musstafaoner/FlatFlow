using Microsoft.AspNetCore.Mvc;
using FlatFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlatFlow.Controllers
{
    public class SiteController : Controller
    {
        private readonly AppDbContext _context;

        public SiteController(AppDbContext context)
        {
            _context = context;
        }

        // Yöneticinin sadece kendi yetkili olduğu siteleri listeler
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var siteler = await _context.Siteler
                .OrderByDescending(s => s.AktifMi)
                .ThenByDescending(s => s.KayitTarihi)
                .ToListAsync();

            // İstatistik hesaplamaları
            ViewBag.ToplamSite = siteler.Count;
            ViewBag.AktifSite = siteler.Count(s => s.AktifMi);

            var aktifSiteIdleri = siteler.Where(s => s.AktifMi).Select(s => s.SiteId).ToList();
            ViewBag.ToplamDaire = await _context.Daireler.CountAsync(d => aktifSiteIdleri.Contains(d.SiteId));

            return View(siteler);
        }

        [HttpPost]
        public async Task<IActionResult> DurumGuncelle(int id)
        {
            var site = await _context.Siteler.FindAsync(id);
            if (site != null)
            {
                site.AktifMi = !site.AktifMi; 
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Site model)
        {
            // 1. Yeni siteyi veritabanına eklemek için
            _context.Siteler.Add(model);
            await _context.SaveChangesAsync(); 

            // 2. Bu siteyi ekleyen yöneticiye yetki verir (Ara tabloya kaydeder)
            var kullaniciIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int kullaniciId = int.Parse(kullaniciIdStr);

            var yetki = new KullaniciSite
            {
                KullaniciId = kullaniciId,
                SiteId = model.SiteId
            };

            _context.KullaniciSiteler.Add(yetki);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Yonet(int id)
        {
            // Seçilen Sitenin ID'sini session hafızasına kaydettim
            HttpContext.Session.SetInt32("AktifSiteId", id);

            // Hafızaya aldıktan sonra dashboarda yönlendirdim
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var site = await _context.Siteler.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }
            return View(site);
        }

        [HttpPost]
        public async Task<IActionResult> Duzenle(Site model)
        {
            var mevcutSite = await _context.Siteler.FindAsync(model.SiteId);

            if (mevcutSite != null)
            {
                mevcutSite.Ad = model.Ad;
                mevcutSite.Adres = model.Adres;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var site = await _context.Siteler.FindAsync(id);

            if (site != null)
            {
                _context.Siteler.Remove(site);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}