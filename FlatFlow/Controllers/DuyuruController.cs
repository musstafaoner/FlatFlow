using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlatFlow.Models;
using System;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Yonetici")] // Sadece yöneticiler duyuru ekleyebilir
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
            var yeniDuyuru = new Duyuru
            {
                Baslik = Baslik,
                Icerik = Icerik,
                OlusturulmaTarihi = DateTime.Now
            };

            _context.Duyurular.Add(yeniDuyuru);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home"); // Ekledikten sonra anasayfaya dön
        }
    }
}