using System;
using FlatFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Yonetici")] 
    public class DuyuruController : Controller
    {
        private readonly AppDbContext _context;

        public DuyuruController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(string Baslik, string Icerik)
        {
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");

            if (aktifSiteId == null || aktifSiteId == 0)
            {
                return RedirectToAction("Index", "Site");
            }

            var yeniDuyuru = new Duyuru
            {
                Baslik = Baslik,
                Icerik = Icerik,
                OlusturulmaTarihi = DateTime.Now,
                SiteId = aktifSiteId.Value 
            };

            _context.Duyurular.Add(yeniDuyuru);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? aktifSiteId = HttpContext.Session.GetInt32("AktifSiteId");
            if (aktifSiteId == null || aktifSiteId == 0) return RedirectToAction("Index", "Site");

            var duyurular = await _context.Duyurular
                .Where(d => d.SiteId == aktifSiteId)
                .OrderByDescending(d => d.OlusturulmaTarihi)
                .ToListAsync();

            return View(duyurular);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var duyuru = await _context.Duyurular.FindAsync(id);
            if (duyuru == null) return NotFound();

            return View(duyuru);
        }

        [HttpPost]
        public async Task<IActionResult> Duzenle(Duyuru model)
        {
            var mevcutDuyuru = await _context.Duyurular.FindAsync(model.Id);

            if (mevcutDuyuru != null)
            {
                mevcutDuyuru.Baslik = model.Baslik;
                mevcutDuyuru.Icerik = model.Icerik;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var duyuru = await _context.Duyurular.FindAsync(id);
            if (duyuru != null)
            {
                _context.Duyurular.Remove(duyuru);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}