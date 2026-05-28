using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlatFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Yonetici")]
    public class AidatController : Controller
    {
        private readonly AppDbContext _context;

        public AidatController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string arananBlok, string odemeDurumu)
        {
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");

            if (aktifSiteId == null || aktifSiteId == 0)
            {
                return RedirectToAction("Index", "Site");
            }

            var sorgu = _context.Aidatlar
                .Include(a => a.Daire)
                    .ThenInclude(d => d.Kullanici) 
                .Where(a => a.SiteId == aktifSiteId)
                .AsQueryable();

            ViewBag.Bloklar = await _context.Daireler
                .Where(d => d.SiteId == aktifSiteId)
                .Select(d => d.Blok)
                .Distinct()
                .ToListAsync();

            ViewBag.SeciliBlok = arananBlok;
            ViewBag.SeciliDurum = odemeDurumu;

            if (!string.IsNullOrEmpty(arananBlok))
            {
                sorgu = sorgu.Where(a => a.Daire.Blok == arananBlok);
            }

            if (!string.IsNullOrEmpty(odemeDurumu))
            {
                if (odemeDurumu == "Odenmedi")
                    sorgu = sorgu.Where(a => a.OdendiMi == false);
                else if (odemeDurumu == "Odendi")
                    sorgu = sorgu.Where(a => a.OdendiMi == true);
            }

            var aidatlar = await sorgu
                .OrderByDescending(a => a.Yil)
                .ThenByDescending(a => a.Ay)
                .ToListAsync();

            return View(aidatlar);
        }

        [HttpGet]
        public async Task<IActionResult> Ekle()
        {
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");

            if (aktifSiteId == null || aktifSiteId == 0)
            {
                return RedirectToAction("Index", "Site");
            }

            ViewBag.Daireler = await _context.Daireler
                .Where(d => d.SiteId == aktifSiteId)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(Aidat model)
        {
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");

            if (aktifSiteId == null || aktifSiteId == 0)
            {
                return RedirectToAction("Index", "Site");
            }

            model.SiteId = aktifSiteId.Value;
            model.OdendiMi = false; 

            _context.Aidatlar.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detay(int id)
        {
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");
            if (aktifSiteId == null || aktifSiteId == 0)
            {
                return RedirectToAction("Index", "Site");
            }

            var aidat = await _context.Aidatlar
                .Include(a => a.Daire)
                    .ThenInclude(d => d.Kullanici)
                .FirstOrDefaultAsync(a => a.AidatId == id && a.SiteId == aktifSiteId);

            if (aidat == null)
            {
                return NotFound();
            }

            return View(aidat);
        }

        [HttpPost]
        public async Task<IActionResult> DurumDegistir(int id)
        {
            var aidat = await _context.Aidatlar.FindAsync(id);

            if (aidat != null)
            {
                aidat.OdendiMi = !aidat.OdendiMi;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Detay", new { id = id });
        }
    }
}