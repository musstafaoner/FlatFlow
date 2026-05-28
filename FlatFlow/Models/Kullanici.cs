using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlatFlow.Models
{
    public class Kullanici
    {
        public int KullaniciId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Eposta { get; set; }
        public string Sifre { get; set; }
        public string TelefonNumarasi { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public ICollection<Daire> Daireler { get; set; }
        public ICollection<Odeme> Odemeler { get; set; }
        public ICollection<ArizaTalep> ArizaTalepleri { get; set; }
    }
}