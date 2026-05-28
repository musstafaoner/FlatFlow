namespace FlatFlow.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public string Ad { get; set; }
        public string Adres { get; set; }
        public bool AktifMi { get; set; } = true; 
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        public ICollection<Daire> Daireler { get; set; }
        public ICollection<Duyuru> Duyurular { get; set; }
        public ICollection<KullaniciSite> Yoneticiler { get; set; }
    }
}