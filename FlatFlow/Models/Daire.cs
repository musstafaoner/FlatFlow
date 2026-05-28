namespace FlatFlow.Models
{
    public class Daire
    {
        public int DaireId { get; set; }
        public string Blok { get; set; } 
        public string KapiNumarasi { get; set; }
        public string Tip { get; set; } 
        public bool BosMu { get; set; }

        public int? KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        public ICollection<Aidat> Aidatlar { get; set; }
    }
}