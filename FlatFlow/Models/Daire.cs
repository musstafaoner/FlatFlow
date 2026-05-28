namespace FlatFlow.Models
{
    public class Daire
    {
        public int Id { get; set; }
        public string Blok { get; set; } // A Blok, B Blok vb.
        public string KapiNumarasi { get; set; }
        public string Tip { get; set; } // "2+1", "3+1" vb.
        public bool BosMu { get; set; }

        // Foreign Key
        public int? KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        // Navigation Property
        public ICollection<Aidat> Aidatlar { get; set; }
    }
}