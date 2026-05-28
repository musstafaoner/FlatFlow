namespace FlatFlow.Models
{
    public class Aidat
    {
        public int Id { get; set; }
        public decimal Tutar { get; set; }
        public int Ay { get; set; }
        public int Yil { get; set; }
        public bool OdendiMi { get; set; }

        // Foreign Key
        public int DaireId { get; set; }
        public Daire Daire { get; set; }

        // Navigation Property
        public ICollection<Odeme> Odemeler { get; set; }
    }
}