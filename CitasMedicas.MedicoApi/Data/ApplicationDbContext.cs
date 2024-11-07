using CitasMedicas.MedicoApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CitasMedicas.MedicoApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<HorarioAtencion> HorariosAtencion { get; set; }
        public DbSet<Correlativo> Correlativos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medico>().ToTable("Medico");
            modelBuilder.Entity<HorarioAtencion>().ToTable("HorarioAtencion");
            modelBuilder.Entity<Correlativo>().ToTable("Correlativo");
            base.OnModelCreating(modelBuilder);
        }
    }
}
