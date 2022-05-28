using CheckIn.API.Models;
using CheckIn.API.Models.Apis;
using CheckIn.API.Models.ModelCliente;
using CheckIn.API.Models.ModelMain;
using FEConectorWA.API.Models.Apis;
using Newtonsoft.Json;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;

namespace CheckIn.API.Controllers
{
    [Authorize]

    public class AceptacionController: ApiController
    {
        ModelCliente db;
        G G = new G();
        Metodos metodo = new Metodos();

        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                DateTime time = new DateTime();
                if (filtro.FechaFinal != time)
                {
                    filtro.FechaFinal = filtro.FechaFinal.AddDays(1);

                }
                var Compras = db.BandejaEntrada.Where(a => (filtro.FechaInicio != time ? a.FechaIngreso >= filtro.FechaInicio : true) && (filtro.FechaFinal != time ? a.FechaIngreso <= filtro.FechaFinal : true) && a.XmlConfirmacion != null).ToList();



                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK, Compras);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/Aceptacion/Consultar")]
        public HttpResponseMessage GetOne([FromUri]int id)
        {
            try
            {

                G.AbrirConexionAPP(out db);


                var Documentos = db.BandejaEntrada.Where(a => a.Id == id).FirstOrDefault();


                if (Documentos == null)
                {
                    throw new Exception("Este documento no se encuentra registrado");
                }
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK, Documentos);
            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromBody] BandejaEntrada bandeja)
        {
            G.AbrirConexionAPP(out db);

            var t = db.Database.BeginTransaction();
            try
            {

                if (string.IsNullOrEmpty(bandeja.tipo) || bandeja.tipo == "0" || bandeja.CondicionImpuesto == "0" || string.IsNullOrEmpty(bandeja.CondicionImpuesto) || string.IsNullOrEmpty(bandeja.CodigoActividad))
                {
                    throw new Exception("Faltan datos por rellenar");
                }
                Parametros parametros = db.Parametros.FirstOrDefault();

                var Bandeja = db.BandejaEntrada.Where(a => a.Id == bandeja.Id).FirstOrDefault();

                MakeXMLAceptacionFactura xml = new MakeXMLAceptacionFactura();
                var Sucursal = db.Sucursales.FirstOrDefault();

                xml.api_key = Sucursal.ApiKey;
                //Generacion del nodo clave
                xml.clave = new FEConectorWA.API.Models.Apis.clave();
                xml.clave.tipo = bandeja.tipo;
                xml.clave.sucursal = Sucursal.codSuc;
                xml.clave.terminal = Sucursal.Terminal;
                xml.clave.numero_documento = Bandeja.NumeroConsecutivo;
                xml.clave.numero_cedula_emisor = Bandeja.IdEmisor;
                xml.clave.fecha_emision_doc = Bandeja.FechaEmision.Substring(6, 4) + "-" + Bandeja.FechaEmision.Substring(3, 2) + "-" + Bandeja.FechaEmision.Substring(0, 2) + "T12:00:00-06:00";// DateTime.Parse(Bandeja.FechaEmision).ToString("yyyy-MM-ddThh:mm:ss-06:00");
                xml.clave.mensaje = bandeja.Mensaje;
                xml.clave.detalle_mensaje = bandeja.DetalleMensaje;
                xml.clave.codigo_actividad = bandeja.CodigoActividad;
                xml.clave.condicion_impuesto = bandeja.CondicionImpuesto;
                xml.clave.impuesto_acreditar = Math.Round(bandeja.impuestoAcreditar.Value).ToString();
                xml.clave.gasto_aplicable = Math.Round(bandeja.gastoAplicable.Value).ToString();
                xml.clave.monto_total_impuesto = Math.Round(Bandeja.Impuesto.Value).ToString();
                xml.clave.total_factura = Math.Round(Bandeja.TotalComprobante.Value).ToString();
                xml.clave.numero_cedula_receptor = Sucursal.Cedula;
                xml.clave.num_consecutivo_receptor = Sucursal.consecAFC.ToString();

                db.Entry(Sucursal).State = System.Data.Entity.EntityState.Modified;
                Sucursal.consecAFC += 1;
                db.SaveChanges();

                xml.clave.situacion_presentacion = (DateTime.Now.Date - DateTime.Parse(xml.clave.fecha_emision_doc)).TotalDays == 0 ? "1" : "3";
                xml.clave.codigo_seguridad = metodo.GeneraNumero();

                //Generacion del nodo emisor

                xml.emisor = new FEConectorWA.API.Models.Apis.emisor();
                xml.emisor.identificacion = new FEConectorWA.API.Models.Apis.identificacion();
                xml.emisor.identificacion.tipo = Bandeja.tipoIdentificacionEmisor;
                xml.emisor.identificacion.numero = Bandeja.IdEmisor;

                xml.parametros = new parametros();
                xml.parametros.enviodgt = "A";


                db.Entry(Bandeja).State = System.Data.Entity.EntityState.Modified;
                Bandeja.tipo = bandeja.tipo;
                Bandeja.Mensaje = bandeja.Mensaje;
                Bandeja.DetalleMensaje = bandeja.DetalleMensaje;
                Bandeja.CodigoActividad = bandeja.CodigoActividad;
                Bandeja.CondicionImpuesto = bandeja.CondicionImpuesto;
                Bandeja.impuestoAcreditar = bandeja.impuestoAcreditar;
                Bandeja.gastoAplicable = bandeja.gastoAplicable;
                Bandeja.situacionPresentacion = xml.clave.situacion_presentacion;
                db.SaveChanges();

                HttpClient cliente = new HttpClient();

                var httpContent = new StringContent(JsonConvert.SerializeObject(xml), Encoding.UTF8, "application/json");
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response = await cliente.PutAsync(parametros.urlCyberAceptacion, httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        response.Content.Headers.ContentType.MediaType = "application/json";
                        var resp = await response.Content.ReadAsAsync<respuesta>();



                        db.Entry(Bandeja).State = System.Data.Entity.EntityState.Modified;
                        Bandeja.Procesado = "1";
                        Bandeja.RespuestaHacienda = resp.code.ToString();

                        Bandeja.XMLRespuesta = resp.data;

                        Bandeja.JSON = JsonConvert.SerializeObject(xml);

                        Bandeja.ClaveReceptor = resp.clave;




                        db.SaveChanges();


                    }

                }
                catch (Exception ex)
                {
                    BitacoraErrores be = new BitacoraErrores();
                    be.DocNum = "";
                    be.Type = "";
                    be.Descripcion = ex.Message;
                    be.StackTrace = ex.StackTrace;
                    be.Fecha = DateTime.Now;
                    db.BitacoraErrores.Add(be);
                    db.SaveChanges();
                }

                t.Commit();
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK, Bandeja);
            }
            catch (Exception ex)
            {
                t.Rollback();

                BitacoraErrores be = new BitacoraErrores();
                be.DocNum = "";
                be.Type = "";
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

    }
}