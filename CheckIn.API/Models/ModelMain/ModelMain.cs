namespace CheckIn.API.Models.ModelMain
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelMain : DbContext
    {
        public ModelMain()
            : base("name=ModelMain")
        {
        }

        public virtual DbSet<LicEmpresas> LicEmpresas { get; set; }
        public virtual DbSet<LicUsuarios> LicUsuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LicEmpresas>()
                .Property(e => e.CedulaJuridica)
                .IsUnicode(false);

            modelBuilder.Entity<LicEmpresas>()
                .Property(e => e.NombreEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<LicEmpresas>()
                .Property(e => e.CadenaConexionBD)
                .IsUnicode(false);

            modelBuilder.Entity<LicUsuarios>()
                .Property(e => e.CedulaJuridica)
                .IsUnicode(false);

            modelBuilder.Entity<LicUsuarios>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<LicUsuarios>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<LicUsuarios>()
                .Property(e => e.Clave)
                .IsUnicode(false);
        }
    }
}
