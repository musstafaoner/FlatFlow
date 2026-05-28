namespace FlatFlow.Models
{
    public class Rol
    {
        public int RolId { get; set; }
        public string Ad { get; set; } // Örn: "Yonetici", "Sakin"

        // Navigation Property
        public ICollection<Kullanici> Kullanicilar { get; set; }
    }
}