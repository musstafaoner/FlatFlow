using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlatFlow.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlatFlow.Controllers
{
    [Authorize(Roles = "Sakin")] // SADECE BİNA SAKİNLERİ GİREBİLİR
    public class SakinController : Controller
    {
        private readonly AppDbContext _context;

        public SakinController(AppDbContext context)
        {
            _context = context;
        }

        // 1. SAKİNİN KENDİ BORÇLARINI LİSTELEME EKRANI
        public async Task<IActionResult> Borclarim()
        {
            // Giriş yapan kullanıcının ID'sini çerezden (Claims) çekiyoruz
            var kullaniciIdUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (kullaniciIdUserId == null) return RedirectToAction("GirisYap", "Hesap");

            int id = int.Parse(kullaniciIdUserId);

            // Veritabanından sadece bu kullanıcıya ait dairelerin aidatlarını getiriyoruz
            var borclar = await _context.Aidatlar
                .Include(a => a.Daire)
                .Where(a => a.Daire.KullaniciId == id)
                .ToListAsync();

            return View(borclar);
        }

        // 2. ÖDEME YAPMA SİMÜLASYONU (POST)
        // Sakin "Öde" butonuna bastığında aidat durumunu "Ödendi" yapacak ve Ödemeler tablosuna kayıt atacak
        [HttpPost]
        public async Task<IActionResult> OdemeYap(int aidatId)
        {
            var aidat = await _context.Aidatlar.FindAsync(aidatId);
            var kullaniciIdUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (aidat != null && kullaniciIdUserId != null)
            {
                // 1. Aidat durumunu ödendi yap
                aidat.OdendiMi = true;

                // 2. Ödemeler tablosuna (6. tablomuz) log kaydı at
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

        // SAKİNİN KENDİ OLUŞTURDUĞU ARIZALARI LİSTELEME
        public async Task<IActionResult> Arizalarim()
        {
            var kullaniciId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var arizalar = await _context.ArizaTalepleri
                .Where(a => a.KullaniciId == int.Parse(kullaniciId))
                .OrderByDescending(a => a.OlusturulmaTarihi)
                .ToListAsync();

            return View(arizalar);
        }

        // YENİ ARIZA BİLDİRME EKRANI (GET)
        [HttpGet]
        public IActionResult ArizaBildir()
        {
            return View();
        }

        // YENİ ARIZA BİLDİRME İŞLEMİ (POST)
        [HttpPost]
        public async Task<IActionResult> ArizaBildir(string Baslik, string Aciklama)
        {
            var kullaniciId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var yeniAriza = new ArizaTalep
            {
                Baslik = Baslik,
                Aciklama = Aciklama,
                Durum = "Beklemede", // Varsayılan durum
                OlusturulmaTarihi = System.DateTime.Now,
                KullaniciId = int.Parse(kullaniciId)
            };

            _context.ArizaTalepleri.Add(yeniAriza);
            await _context.SaveChangesAsync();

            return RedirectToAction("Arizalarim");
        }
    }
}