namespace CheckIn.API.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CondicionesVenta")]
    public partial class CondicionesVenta
    {
        public int id { get; set; }

        [StringLength(2)]
        public string codCyber { get; set; }

        [StringLength(100)]
        public string Nombre { get; set; }
    }
}
