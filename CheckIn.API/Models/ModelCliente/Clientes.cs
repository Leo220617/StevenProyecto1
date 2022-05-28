namespace CheckIn.API.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Clientes
    {
        public int id { get; set; }

        [StringLength(200)]
        public string Nombre { get; set; }

        [StringLength(12)]
        public string Cedula { get; set; }

        [StringLength(2)]
        public string TipoCedula { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string CodPais { get; set; }

        [StringLength(50)]
        public string Telefono { get; set; }

        public int? provincia { get; set; }

        [StringLength(2)]
        public string canton { get; set; }

        [StringLength(2)]
        public string distrito { get; set; }

        [StringLength(2)]
        public string barrio { get; set; }

        [StringLength(250)]
        public string sennas { get; set; }
    }
}
