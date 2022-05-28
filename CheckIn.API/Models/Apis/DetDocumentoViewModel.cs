using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CheckIn.API.Models.Apis
{
    public class DetDocumentoViewModel
    {
        public int id { get; set; }

        public int idEncabezado { get; set; }

        public int? NumLinea { get; set; }

        [StringLength(15)]
        public string partidaArancelaria { get; set; }

        [Column(TypeName = "money")]
        public decimal? exportacion { get; set; }

        [StringLength(13)]
        public string CodCabys { get; set; }

        [StringLength(2)]
        public string tipoCod { get; set; }

        [StringLength(20)]
        public string codPro { get; set; }

        [Column(TypeName = "money")]
        public decimal? cantidad { get; set; }

        [StringLength(5)]
        public string unidadMedida { get; set; }

        [StringLength(20)]
        public string unidadMedidaComercial { get; set; }

        [StringLength(200)]
        public string NomPro { get; set; }

        [Column(TypeName = "money")]
        public decimal? PrecioUnitario { get; set; }

        [Column(TypeName = "money")]
        public decimal? MontoTotal { get; set; }

        public int PorDesc { get; set; }

        [Column(TypeName = "money")]
        public decimal? MontoDescuento { get; set; }

        [StringLength(80)]
        public string NaturalezaDescuento { get; set; }

        [Column(TypeName = "money")]
        public decimal? SubTotal { get; set; }

        [Column(TypeName = "money")]
        public decimal? baseImponible { get; set; }

        [StringLength(10)]
        public string idImpuesto { get; set; }

        [Column(TypeName = "money")]
        public decimal? montoImpuesto { get; set; }

        [Column(TypeName = "money")]
        public decimal? factorIVA { get; set; }

        [StringLength(2)]
        public string exonTipoDoc { get; set; }

        [StringLength(40)]
        public string exonNumdoc { get; set; }

        [StringLength(160)]
        public string exonNomInst { get; set; }

        public DateTime? exonFecEmi { get; set; }

        public int? exonPorExo { get; set; }

        [Column(TypeName = "money")]
        public decimal? exonMonExo { get; set; }

        [Column(TypeName = "money")]
        public decimal? impNeto { get; set; }

        [Column(TypeName = "money")]
        public decimal? totalLinea { get; set; }



    }
}