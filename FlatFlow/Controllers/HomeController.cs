using System.Security.Claims;
using FlatFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            var kullaniciIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int kullaniciId = int.Parse(kullaniciIdStr ?? "0");

            int? aktifSiteId = null;
            string aktifSiteAdi = "Sistem";

            if (User.IsInRole("Yonetici"))
            {
                aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");

                if (aktifSiteId == null || aktifSiteId == 0)
                {
                    return RedirectToAction("Index", "Site");
                }

                var site = await _context.Siteler.FindAsync(aktifSiteId);
                aktifSiteAdi = site?.Ad ?? "Sistem";
            }
            else
            {
                var daire = await _context.Daireler
                    .Include(d => d.Site)
                    .FirstOrDefaultAsync(d => d.KullaniciId == kullaniciId);

                if (daire != null)
                {
                    aktifSiteId = daire.SiteId;
                    aktifSiteAdi = daire.Site.Ad;
                }
            }

            ViewBag.AktifSiteAdi = aktifSiteAdi;

            var duyurular = new List<Duyuru>();

            if (aktifSiteId.HasValue && aktifSiteId > 0)
            {
                ViewBag.ToplamDaire = await _context.Daireler.CountAsync(d => d.SiteId == aktifSiteId);
                ViewBag.DoluDaire = await _context.Daireler.CountAsync(d => d.SiteId == aktifSiteId && d.KullaniciId != null);
                ViewBag.BekleyenAriza = await _context.ArizaTalepleri.CountAsync(a => a.SiteId == aktifSiteId && a.Durum == "Beklemede");

                var odenenAidatlar = await _context.Aidatlar.Where(a => a.SiteId == aktifSiteId && a.OdendiMi).ToListAsync();
                ViewBag.Kasa = odenenAidatlar.Any() ? odenenAidatlar.Sum(a => a.Tutar) : 0;

                duyurular = await _context.Duyurular
                    .Where(d => d.SiteId == aktifSiteId)
                    .OrderByDescending(d => d.OlusturulmaTarihi)
                    .Take(5)
                    .ToListAsync();
            }
            else
            {
                ViewBag.ToplamDaire = 0; ViewBag.DoluDaire = 0; ViewBag.BekleyenAriza = 0; ViewBag.Kasa = 0;
            }

            return View("Dashboard", duyurular);
        }
    }
}