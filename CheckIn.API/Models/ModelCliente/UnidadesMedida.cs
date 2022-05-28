namespace CheckIn.API.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnidadesMedida")]
    public partial class UnidadesMedida
    {
        public int id { get; set; }

        [StringLength(7)]
        public string codCyber { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }
    }
}
