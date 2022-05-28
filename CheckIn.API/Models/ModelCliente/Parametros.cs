namespace CheckIn.API.Models.ModelCliente
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Parametros
    {
        public int id { get; set; }

        [StringLength(500)]
        public string urlCyber { get; set; }

        [StringLength(500)]
        public string urlCyberRespHacienda { get; set; }

        [StringLength(500)]
        public string urlCyberAceptacion { get; set; }

        [StringLength(500)]
        public string urlCyberReenvio { get; set; }

        [StringLength(500)]
        public string urlWebApi { get; set; }

        public string urlCyberConsultaHacienda { get; set; }
    }
}
