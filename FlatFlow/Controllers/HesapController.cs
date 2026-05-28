using Microsoft.AspNetCore.Mvc;
using FlatFlow.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Controllers
{
    public class HesapController : Controller
    {
        private readonly AppDbContext _context;

        public HesapController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GirisYap(GirisViewModel model)
        {          
            var kullanici = await _context.Kullanicilar
                .Include(k => k.Rol)
                .FirstOrDefaultAsync(k => k.Eposta == model.Eposta && k.Sifre == model.Sifre);

            if (kullanici != null)
            {               
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, kullanici.KullaniciId.ToString()),
                    new Claim(ClaimTypes.Name, kullanici.Ad + " " + kullanici.Soyad),
                    new Claim(ClaimTypes.Email, kullanici.Eposta),
                    new Claim(ClaimTypes.Role, kullanici.Rol.Ad) 
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home"); 
            }

            ViewBag.Hata = "E-posta veya şifre hatalı!";
            return View(model);
        }

        public async Task<IActionResult> CikisYap()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> KayitOl(KayitViewModel model)
        {
            if (model.Sifre != model.SifreTekrar)
            {
                ViewBag.Hata = "Şifreler birbiriyle uyuşmuyor!";
                return View(model);
            }

            var epostaVarMi = await _context.Kullanicilar.AnyAsync(x => x.Eposta == model.Eposta);
            if (epostaVarMi)
            {
                ViewBag.Hata = "Bu e-posta adresi zaten sistemde kayıtlı!";
                return View(model);
            }

            var rol = await _context.Roller.FirstOrDefaultAsync(r => r.Ad == model.SecilenRol);
            if (rol == null)
            {
                rol = new Rol { Ad = model.SecilenRol };
                _context.Roller.Add(rol);
                await _context.SaveChangesAsync();
            }

            var yeniKullanici = new Kullanici
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                Eposta = model.Eposta,
                TelefonNumarasi = model.TelefonNumarasi?.Replace(" ", "").Replace("+", ""),
                Sifre = model.Sifre,
                RolId = rol.RolId
            };

            _context.Kullanicilar.Add(yeniKullanici);
            await _context.SaveChangesAsync();

            return RedirectToAction("GirisYap");
        }

        [HttpGet]
        public async Task<IActionResult> Profil()
        {
            var kullaniciIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(kullaniciIdStr))
            {
                return RedirectToAction("GirisYap");
            }

            int kullaniciId = int.Parse(kullaniciIdStr);
            var kullanici = await _context.Kullanicilar.FindAsync(kullaniciId);

            if (kullanici == null)
            {
                return NotFound();
            }

            return View(kullanici);
        }

        [HttpPost]
        public async Task<IActionResult> Profil(Kullanici model, string? YeniSifre)
        {
            var kullaniciIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(kullaniciIdStr)) return RedirectToAction("GirisYap");

            int kullaniciId = int.Parse(kullaniciIdStr);
            var mevcutKullanici = await _context.Kullanicilar.FindAsync(kullaniciId);

            if (mevcutKullanici != null)
            {
                mevcutKullanici.Ad = model.Ad;
                mevcutKullanici.Soyad = model.Soyad;
                mevcutKullanici.Eposta = model.Eposta;
                mevcutKullanici.TelefonNumarasi = model.TelefonNumarasi;

                if (!string.IsNullOrEmpty(YeniSifre))
                {
                    mevcutKullanici.Sifre = YeniSifre;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}