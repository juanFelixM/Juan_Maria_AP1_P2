using Microsoft.EntityFrameworkCore;
using Juan_Maria_AP1_P2.Models;

namespace Juan_Maria_AP1_P2.DAL
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Entradas> Entradas { get; set; }
        public virtual DbSet<EntradasDetalle> EntradasDetalle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntradasDetalle>()
                .HasOne(ed => ed.Producto)
                .WithMany(p => p.EntradasDetalle)
                .HasForeignKey(ed => ed.ProductoId);

            modelBuilder.Entity<Entradas>()
                .HasMany(e => e.EntradasDetalle)
                .WithOne()
                .HasForeignKey(ed => ed.EntradasId);

            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    ProductosId = 1,
                    Descripcion = "Mani",
                    Peso = 12
                },
                new Producto
                {
                    ProductosId = 2,
                    Descripcion = "Almendra",
                    Peso = 17
                },
                new Producto
                {
                    ProductosId = 3,
                    Descripcion = "Pistacho",
                    Peso = 3
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
