namespace CheckIn.API.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BandejaEntrada")]
    public partial class BandejaEntrada
    {
        public int Id { get; set; }

        public byte[] XmlFactura { get; set; }

        public string XmlConfirmacion { get; set; }

        public byte[] Pdf { get; set; }

        public DateTime FechaIngreso { get; set; }

        [StringLength(1)]
        public string Procesado { get; set; }

        public DateTime? FechaProcesado { get; set; }

        public string Mensaje { get; set; }

        public string Asunto { get; set; }

        public string Remitente { get; set; }

        [StringLength(100)]
        public string NumeroConsecutivo { get; set; }

        [StringLength(20)]
        public string TipoDocumento { get; set; }

        [StringLength(20)]
        public string FechaEmision { get; set; }

        [StringLength(200)]
        public string NombreEmisor { get; set; }

        [StringLength(100)]
        public string IdEmisor { get; set; }

        [StringLength(20)]
        public string CodigoMoneda { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalComprobante { get; set; }

        [StringLength(3)]
        public string tipo { get; set; }

        [Column(TypeName = "money")]
        public decimal? Impuesto { get; set; }

        [StringLength(500)]
        public string DetalleMensaje { get; set; }

        [StringLength(20)]
        public string CodigoActividad { get; set; }

        [StringLength(3)]
        public string CondicionImpuesto { get; set; }

        [Column(TypeName = "money")]
        public decimal? impuestoAcreditar { get; set; }

        [Column(TypeName = "money")]
        public decimal? gastoAplicable { get; set; }

        [StringLength(2)]
        public string situacionPresentacion { get; set; }

        [StringLength(3)]
        public string tipoIdentificacionEmisor { get; set; }

        public string JSON { get; set; }

        [StringLength(50)]
        public string RespuestaHacienda { get; set; }

        public string XMLRespuesta { get; set; }

        [StringLength(100)]
        public string ClaveReceptor { get; set; }

        [StringLength(50)]
        public string ConsecutivoReceptor { get; set; }

        public decimal IVA0 { get; set; }
        public decimal IVA1 { get; set; }

        public decimal IVA2 { get; set; }

        public decimal IVA4 { get; set; }

        public decimal IVA8 { get; set; }

        public decimal IVA13 { get; set; }



    }
}
