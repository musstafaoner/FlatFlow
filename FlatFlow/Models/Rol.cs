namespace FlatFlow.Models
{
    public class Rol
    {
        public int RolId { get; set; }
        public string Ad { get; set; } 

        public ICollection<Kullanici> Kullanicilar { get; set; }
    }
}