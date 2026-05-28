namespace FlatFlow.Models
{
    public class Odeme
    {
        public int OdemeId { get; set; }
        public decimal OdenenTutar { get; set; }
        public System.DateTime OdemeTarihi { get; set; }

        public int AidatId { get; set; }
        public Aidat Aidat { get; set; }

        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}