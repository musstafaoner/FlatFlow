namespace FlatFlow.Models
{
    public class ArizaTalep
    {
        public int ArizaTalepId { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Durum { get; set; } 
        public System.DateTime OlusturulmaTarihi { get; set; }
        public int SiteId { get; set; }
        public Site Site { get; set; }
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}