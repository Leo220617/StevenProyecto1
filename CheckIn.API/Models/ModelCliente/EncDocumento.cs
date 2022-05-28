namespace CheckIn.API.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EncDocumento")]
    public partial class EncDocumento
    {
        public int id { get; set; }

        [Required]
        [StringLength(3)]
        public string idSucursal { get; set; }

        public int? consecutivoInterno { get; set; }

        [StringLength(2)]
        public string TipoDocumento { get; set; }

        public DateTime? Fecha { get; set; }

        [StringLength(50)]
        public string CodActividadEconomica { get; set; }

        public int? CodCliente { get; set; }

        [StringLength(200)]
        public string NombreCliente { get; set; }

        [StringLength(12)]
        public string CedulaCliente { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(2)]
        public string TipoIdentificacion { get; set; }

        public int? condicionVenta { get; set; }

        public int? plazoCredito { get; set; }

        [StringLength(15)]
        public string medioPago { get; set; }

        [Column(TypeName = "money")]
        public decimal? montoOtrosCargos { get; set; }

        [StringLength(3)]
        public string moneda { get; set; }

        [Column(TypeName = "money")]
        public decimal? tipoCambio { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalserviciogravado { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalservicioexento { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalservicioexonerado { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalmercaderiagravado { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalmercaderiaexonerado { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalmercaderiaexenta { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalgravado { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalexento { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalexonerado { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalventa { get; set; }

        [Column(TypeName = "money")]
        public decimal? totaldescuentos { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalventaneta { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalimpuestos { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalivadevuelto { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalotroscargos { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalcomprobante { get; set; }

        [StringLength(2)]
        public string RefTipoDocumento { get; set; }

        [StringLength(50)]
        public string RefNumeroDocumento { get; set; }

        public DateTime? RefFechaEmision { get; set; }

        [StringLength(2)]
        public string RefCodigo { get; set; }

        [StringLength(180)]
        public string RefRazon { get; set; }

        public bool? procesadaHacienda { get; set; }

        [StringLength(500)]
        public string RespuestaHacienda { get; set; }

        public string XMLFirmado { get; set; }

        [StringLength(55)]
        public string ClaveHacienda { get; set; }

        [StringLength(25)]
        public string ConsecutivoHacienda { get; set; }

        public string ErrorCyber { get; set; }

        public int? code { get; set; }

        public string JSON { get; set; }
    }
}
