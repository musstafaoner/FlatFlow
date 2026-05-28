namespace FlatFlow.Models
{
    public class ArizaTalep
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Durum { get; set; } // "Beklemede", "Cozuluyor", "Cozuldu"
        public System.DateTime OlusturulmaTarihi { get; set; }

        // Foreign Key
        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}