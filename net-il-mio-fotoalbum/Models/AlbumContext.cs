using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum
{
    public class AlbumContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=imagesalbumDB;Integrated Security=True;TrustServerCertificate=True").LogTo(s => Debug.WriteLine(s));
        }
    }
}