using BackendCancha.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendCancha.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){ }
        public DbSet<Canchas> Canchas { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<CategoriaP> CategoriaP { get; set; }

        public DbSet<Reserva> Reserva { get; set; } = default!;
        public DbSet<ReservaDetalle> ReservaDetalle { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuario admin inicial
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Admin",
                    Apellido = "Principal",
                    Correo = "admin@example.com",
                    Telefono = "1234567890",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Rol = "admin"
                }
            );
        }

    }
}
    

      

