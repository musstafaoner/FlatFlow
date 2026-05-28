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
    }
}