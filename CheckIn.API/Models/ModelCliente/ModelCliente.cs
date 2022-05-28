namespace CheckIn.API.Models.ModelCliente
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelCliente : DbContext
    {
        public ModelCliente(string connectionString, bool lazyLoadinEnabled = true)
            : base("name=ModelCliente")
        {
            this.Database.Connection.ConnectionString = connectionString;

            try
            {
                this.Database.Connection.Open();
                this.Database.CommandTimeout = 300;

                this.Configuration.LazyLoadingEnabled = lazyLoadinEnabled;
            }
            catch { }
        }

        public virtual DbSet<BandejaEntrada> BandejaEntrada { get; set; }
        public virtual DbSet<Barrios> Barrios { get; set; }
        public virtual DbSet<BitacoraErrores> BitacoraErrores { get; set; }
        public virtual DbSet<Cantones> Cantones { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<CondicionesVenta> CondicionesVenta { get; set; }
        public virtual DbSet<CorreosRecepcion> CorreosRecepcion { get; set; }
        public virtual DbSet<DetDocumento> DetDocumento { get; set; }
        public virtual DbSet<Distritos> Distritos { get; set; }
        public virtual DbSet<EncDocumento> EncDocumento { get; set; }
        public virtual DbSet<Impuestos> Impuestos { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<OtrosCargos> OtrosCargos { get; set; }
        public virtual DbSet<OtrosTextos> OtrosTextos { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<RespuestasCyber> RespuestasCyber { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SeguridadModulos> SeguridadModulos { get; set; }
        public virtual DbSet<SeguridadRolesModulos> SeguridadRolesModulos { get; set; }
        public virtual DbSet<Sucursales> Sucursales { get; set; }
        public virtual DbSet<UnidadesMedida> UnidadesMedida { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.XmlConfirmacion)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.Procesado)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.Mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.Asunto)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.Remitente)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.NumeroConsecutivo)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.TipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.FechaEmision)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.NombreEmisor)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.IdEmisor)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.CodigoMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.TotalComprobante)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.tipo)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.Impuesto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.DetalleMensaje)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.CodigoActividad)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.CondicionImpuesto)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.impuestoAcreditar)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.gastoAplicable)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.situacionPresentacion)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.tipoIdentificacionEmisor)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.JSON)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.RespuestaHacienda)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.XMLRespuesta)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.ClaveReceptor)
                .IsUnicode(false);

            modelBuilder.Entity<BandejaEntrada>()
                .Property(e => e.ConsecutivoReceptor)
                .IsUnicode(false);

            modelBuilder.Entity<Barrios>()
                .Property(e => e.NomBarrio)
                .IsUnicode(false);

            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.DocNum)
                .IsUnicode(false);

            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.StackTrace)
                .IsUnicode(false);

            modelBuilder.Entity<Cantones>()
                .Property(e => e.NomCanton)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Cedula)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.TipoCedula)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.CodPais)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Telefono)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.canton)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.distrito)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.barrio)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.sennas)
                .IsUnicode(false);

            modelBuilder.Entity<CondicionesVenta>()
                .Property(e => e.codCyber)
                .IsUnicode(false);

            modelBuilder.Entity<CondicionesVenta>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<CorreosRecepcion>()
                .Property(e => e.RecepcionEmail)
                .IsUnicode(false);

            modelBuilder.Entity<CorreosRecepcion>()
                .Property(e => e.RecepcionPassword)
                .IsUnicode(false);

            modelBuilder.Entity<CorreosRecepcion>()
                .Property(e => e.RecepcionHostName)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.partidaArancelaria)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.exportacion)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.CodCabys)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.tipoCod)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.codPro)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.cantidad)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.unidadMedida)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.unidadMedidaComercial)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.NomPro)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.PrecioUnitario)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.MontoTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.MontoDescuento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.NaturalezaDescuento)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.SubTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.baseImponible)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.idImpuesto)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.montoImpuesto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.factorIVA)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.exonTipoDoc)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.exonNumdoc)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.exonNomInst)
                .IsUnicode(false);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.exonMonExo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.impNeto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DetDocumento>()
                .Property(e => e.totalLinea)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Distritos>()
                .Property(e => e.NomDistrito)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.idSucursal)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.TipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.CodActividadEconomica)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.NombreCliente)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.CedulaCliente)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.TipoIdentificacion)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.medioPago)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.montoOtrosCargos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.moneda)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.tipoCambio)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalserviciogravado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalservicioexento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalservicioexonerado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalmercaderiagravado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalmercaderiaexonerado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalmercaderiaexenta)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalgravado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalexento)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalexonerado)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalventa)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totaldescuentos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalventaneta)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalimpuestos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalivadevuelto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalotroscargos)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.totalcomprobante)
                .HasPrecision(19, 4);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.RefTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.RefNumeroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.RefCodigo)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.RefRazon)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.RespuestaHacienda)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.XMLFirmado)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.ClaveHacienda)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.ConsecutivoHacienda)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.ErrorCyber)
                .IsUnicode(false);

            modelBuilder.Entity<EncDocumento>()
                .Property(e => e.JSON)
                .IsUnicode(false);

            modelBuilder.Entity<Impuestos>()
                .Property(e => e.id)
                .IsUnicode(false);

            modelBuilder.Entity<Impuestos>()
                .Property(e => e.codigo)
                .IsUnicode(false);

            modelBuilder.Entity<Impuestos>()
                .Property(e => e.codigoTarifa)
                .IsUnicode(false);

            modelBuilder.Entity<Impuestos>()
                .Property(e => e.tarifa)
                .HasPrecision(4, 2);

            modelBuilder.Entity<Login>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Login>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Login>()
                .Property(e => e.Clave)
                .IsUnicode(false);

            modelBuilder.Entity<OtrosCargos>()
                .Property(e => e.tipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<OtrosCargos>()
                .Property(e => e.detalle)
                .IsUnicode(false);

            modelBuilder.Entity<OtrosCargos>()
                .Property(e => e.porcentaje)
                .HasPrecision(5, 5);

            modelBuilder.Entity<OtrosCargos>()
                .Property(e => e.monto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OtrosTextos>()
                .Property(e => e.codigo)
                .IsUnicode(false);

            modelBuilder.Entity<OtrosTextos>()
                .Property(e => e.detalle)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.urlCyber)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.urlCyberRespHacienda)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.urlCyberAceptacion)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.urlCyberReenvio)
                .IsUnicode(false);

            modelBuilder.Entity<Parametros>()
                .Property(e => e.urlWebApi)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.TipoCodigo)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Codigo)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.PrecioUnitario)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Cabys)
                .IsUnicode(false);

            modelBuilder.Entity<RespuestasCyber>()
                .Property(e => e.Detalle)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.NombreRol)
                .IsUnicode(false);

            modelBuilder.Entity<SeguridadModulos>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.codSuc)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.NombreComercial)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Terminal)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.TipoCedula)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Cedula)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Provincia)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Canton)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Distrito)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Barrio)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.sennas)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Telefono)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Correo)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.Logo)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.ApiKey)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.codPais)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursales>()
                .Property(e => e.CodActividadComercial)
                .IsUnicode(false);

            modelBuilder.Entity<UnidadesMedida>()
                .Property(e => e.codCyber)
                .IsUnicode(false);

            modelBuilder.Entity<UnidadesMedida>()
                .Property(e => e.Nombre)
                .IsUnicode(false);
        }
    }
}
