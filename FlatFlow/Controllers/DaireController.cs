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

        [HttpGet]
        public async Task<IActionResult> Index(string arananBlok)
        {
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");

            if (aktifSiteId == null || aktifSiteId == 0)
            {
                return RedirectToAction("Index", "Site");
            }

            var sorgu = _context.Daireler
                .Include(d => d.Kullanici)
                .Where(d => d.SiteId == aktifSiteId);

            var bloklar = await sorgu.Select(d => d.Blok).Distinct().ToListAsync();
            ViewBag.Bloklar = bloklar;

            ViewBag.SeciliBlok = arananBlok;

            if (!string.IsNullOrEmpty(arananBlok))
            {
                sorgu = sorgu.Where(d => d.Blok == arananBlok);
            }

            var daireler = await sorgu.ToListAsync();

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
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");

            if (aktifSiteId == null || aktifSiteId == 0)
            {
                return RedirectToAction("Index", "Site");
            }

            model.SiteId = aktifSiteId.Value;

            model.BosMu = !model.KullaniciId.HasValue;

            _context.Daireler.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var daire = await _context.Daireler.FindAsync(id);
            if (daire == null)
            {
                return NotFound();
            }

            ViewBag.Sakinler = await _context.Kullanicilar
                .Include(k => k.Rol)
                .Where(k => k.Rol.Ad == "Sakin")
                .ToListAsync();

            return View(daire);
        }

        [HttpPost]
        public async Task<IActionResult> Duzenle(Daire model)
        {
            var mevcutDaire = await _context.Daireler.FindAsync(model.DaireId);
            if (mevcutDaire == null)
            {
                return NotFound();
            }

            mevcutDaire.Blok = model.Blok;
            mevcutDaire.KapiNumarasi = model.KapiNumarasi;
            mevcutDaire.Tip = model.Tip;
            mevcutDaire.KullaniciId = model.KullaniciId;

            mevcutDaire.BosMu = !model.KullaniciId.HasValue;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}