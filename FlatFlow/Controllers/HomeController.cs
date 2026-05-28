using FlatFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Controllers
{
    // [Authorize] etiketini sildik. Artık anasayfaya herkes girebilir.
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. EĞER KULLANICI GİRİŞ YAPMIŞSA DOĞRUDAN DASHBOARD'A GİTSİN
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ToplamDaire = await _context.Daireler.CountAsync();
                ViewBag.DoluDaire = await _context.Daireler.CountAsync(d => !d.BosMu);
                ViewBag.BekleyenAriza = await _context.ArizaTalepleri.CountAsync(a => a.Durum == "Beklemede");

                var odenenAidatlar = await _context.Aidatlar.Where(a => a.OdendiMi).ToListAsync();
                ViewBag.Kasa = odenenAidatlar.Any() ? odenenAidatlar.Sum(a => a.Tutar) : 0;

                var duyurular = await _context.Duyurular
                    .OrderByDescending(d => d.OlusturulmaTarihi)
                    .Take(5)
                    .ToListAsync();

                // Kullanıcıyı yeni adlandırdığımız Dashboard.cshtml'ye yönlendir
                return View("Dashboard", duyurular);
            }

            // 2. EĞER GİRİŞ YAPILMAMIŞSA TANITIM VİTRİNİ AÇILSIN
            return View();
        }
    }
}