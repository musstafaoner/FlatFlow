namespace FlatFlow.Models
{
    public class Odeme
    {
        public int Id { get; set; }
        public decimal OdenenTutar { get; set; }
        public System.DateTime OdemeTarihi { get; set; }

        // Foreign Keys
        public int AidatId { get; set; }
        public Aidat Aidat { get; set; }

        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}