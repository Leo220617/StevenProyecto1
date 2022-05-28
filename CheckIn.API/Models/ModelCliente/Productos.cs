namespace CheckIn.API.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Productos
    {
        public int id { get; set; }

        [StringLength(2)]
        public string TipoCodigo { get; set; }

        [StringLength(20)]
        public string Codigo { get; set; }

        public int? UnidadMedida { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [Column(TypeName = "money")]
        public decimal? PrecioUnitario { get; set; }

        public int? Impuesto { get; set; }

        [StringLength(13)]
        public string Cabys { get; set; }
    }
}
