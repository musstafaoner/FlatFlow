using Microsoft.EntityFrameworkCore;

namespace FlatFlow.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Rol> Roller { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Daire> Daireler { get; set; }
        public DbSet<Aidat> Aidatlar { get; set; }
        public DbSet<Odeme> Odemeler { get; set; }
        public DbSet<ArizaTalep> ArizaTalepleri { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }
        public DbSet<Site> Siteler { get; set; }
        public DbSet<KullaniciSite> KullaniciSiteler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Daire>()
                .HasOne(d => d.Site)
                .WithMany(s => s.Daireler)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Duyuru>()
                .HasOne(d => d.Site)
                .WithMany(s => s.Duyurular)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Aidat>()
                .HasOne(a => a.Site)
                .WithMany()
                .HasForeignKey(a => a.SiteId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<ArizaTalep>()
                .HasOne(a => a.Site)
                .WithMany()
                .HasForeignKey(a => a.SiteId)
                .OnDelete(DeleteBehavior.NoAction); 
    }
}