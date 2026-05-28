namespace FlatFlow.Models
{
    public class Aidat
    {
        public int AidatId { get; set; }
        public decimal Tutar { get; set; }
        public int Ay { get; set; }
        public int Yil { get; set; }
        public bool OdendiMi { get; set; }

        public int DaireId { get; set; }
        public Daire Daire { get; set; }

        public ICollection<Odeme> Odemeler { get; set; }
    }
}