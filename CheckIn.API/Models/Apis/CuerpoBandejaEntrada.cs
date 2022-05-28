using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckIn.API.Models.Apis
{
    public class CuerpoBandejaEntrada
    {
        public FacturaElectronica FacturaElectronica { get; set; }
    }
    public class FacturaElectronica
    {
        public DetalleServicio DetalleServicio { get; set; }
    }
    public class DetalleServicio
    {
        public LineaDetalle[] LineaDetalle { get; set; }
    }
    public class LineaDetalle
    {
        public Impuesto Impuesto { get; set; }
    }
    public class Impuesto
    {
        public string Tarifa { get; set; }
        public string Monto { get; set; }
    }
}