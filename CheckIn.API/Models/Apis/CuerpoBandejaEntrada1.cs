using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckIn.API.Models.Apis
{
    public class CuerpoBandejaEntrada1
    {
        public FacturaElectronica1 FacturaElectronica { get; set; }
    }
    public class FacturaElectronica1
    {
        public DetalleServicio1 DetalleServicio { get; set; }
    }
    public class DetalleServicio1
    {
        public LineaDetalle1 LineaDetalle { get; set; }
    }
    public class LineaDetalle1
    {
        public Impuesto1 Impuesto { get; set; }
    }
    public class Impuesto1
    {
        public string Tarifa { get; set; }
        public string Monto { get; set; }
    }
}