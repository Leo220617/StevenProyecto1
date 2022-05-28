using CheckIn.API.Models;
using CheckIn.API.Models.Apis;
using CheckIn.API.Models.ModelCliente;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace CheckIn.API.Controllers
{
    [Authorize]

    public class FacturasController: ApiController
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
                var Documentos = db.EncDocumento.Where(a => (filtro.FechaInicio != time ? a.Fecha >= filtro.FechaInicio : true) && (filtro.FechaFinal != time ? a.Fecha <= filtro.FechaFinal : true) && a.code == 1).ToList();

                if (!string.IsNullOrEmpty(filtro.Texto))
                {
                    Documentos = Documentos.Where(a => a.moneda.ToUpper().Contains(filtro.Texto.ToUpper())).ToList();
                }

                if (!string.IsNullOrEmpty(filtro.Estado))
                {
                    if (filtro.Estado != "NULL")
                    {
                        Documentos = Documentos.Where(a => a.RespuestaHacienda != null).ToList();
                        Documentos = Documentos.Where(a => a.RespuestaHacienda.ToUpper().Contains(filtro.Estado.ToUpper())).ToList();

                    }
                }
                if (!string.IsNullOrEmpty(filtro.CodMoneda))
                {
                    if (filtro.CodMoneda != "NULL")
                    {
                        Documentos = Documentos.Where(a => a.TipoDocumento.ToUpper().Contains(filtro.CodMoneda.ToUpper())).ToList();

                    }
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

        [Route("api/Facturas/Consultar")]
        public HttpResponseMessage GetOne([FromUri]int id)
        {
            try
            {

                G.AbrirConexionAPP(out db);


                var Documentos = db.EncDocumento.Where(a => a.id == id).FirstOrDefault();


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


        [Route("api/Detalles/Consultar")]
        public HttpResponseMessage GetOneDetalle([FromUri]int id)
        {
            try
            {

                G.AbrirConexionAPP(out db);


                var Documentos = db.DetDocumento.Where(a => a.idEncabezado == id).ToList();


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

        /////Reenvio de facturas
        ///
        [Route("api/Facturas/Reenvio")]
        [HttpGet]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetReenvioAsync([FromUri] int id, string Sucursal = "001", string Correo = "")
        {

            try
            {
                G.AbrirConexionAPP(out db);

                var item = db.EncDocumento.Where(a => a.id == id).FirstOrDefault();

                var parametros = db.Parametros.FirstOrDefault();
                var sucursal = db.Sucursales.Where(a => a.codSuc == Sucursal).FirstOrDefault();
                if (item != null)
                {
                    var cuerpo = new MakeXMLReenvioFacturas();
                    cuerpo.api_key = sucursal.ApiKey;
                    cuerpo.clave = item.ClaveHacienda;
                    if (string.IsNullOrEmpty(Correo))
                    {
                        cuerpo.correos = item.Email;

                    }
                    else
                    {
                        cuerpo.correos = Correo;
                    }



                    HttpClient cliente2 = new HttpClient();



                    var httpContent2 = new StringContent(JsonConvert.SerializeObject(cuerpo), Encoding.UTF8, "application/json");
                    cliente2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response2 = await cliente2.PostAsync(parametros.urlCyberReenvio, httpContent2);
                        if (response2.IsSuccessStatusCode)
                        {
                            response2.Content.Headers.ContentType.MediaType = "application/json";
                            var resp2 = await response2.Content.ReadAsStringAsync();
                            G.CerrarConexionAPP(db);

                            return Request.CreateResponse(HttpStatusCode.OK, item);
                        }
                        else
                        {
                            G.CerrarConexionAPP(db);

                            return Request.CreateResponse(HttpStatusCode.OK, item);
                        }
                    }
                    catch (Exception ex)
                    {
                        G.CerrarConexionAPP(db);

                        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

                    }
                }




                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK);
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
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }
    }
}