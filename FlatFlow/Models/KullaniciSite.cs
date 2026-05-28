namespace FlatFlow.Models
{
    public class KullaniciSite
    {
        public int KullaniciSiteId { get; set; }

        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}