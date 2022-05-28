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
    public class CondicionesVentaController : ApiController
    {

        ModelCliente db;
        G G = new G();

        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var cond = db.CondicionesVenta.ToList();

                if (!string.IsNullOrEmpty(filtro.Texto))
                {
                    cond = cond.Where(a => a.Nombre.ToUpper().Contains(filtro.Texto.ToUpper())).ToList();
                }


                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK, cond);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/CondicionesVenta/Consultar")]
        public HttpResponseMessage GetOne([FromUri]string id)
        {
            try
            {

                G.AbrirConexionAPP(out db);


                var cond = db.CondicionesVenta.Where(a => a.codCyber == id).FirstOrDefault();


                if (cond == null)
                {
                    throw new Exception("Esta condicion de venta no se encuentra registrado");
                }
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK, cond);
            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] CondicionesVenta cond)
        {
            try
            {

                G.AbrirConexionAPP(out db);

                var Cond = db.CondicionesVenta.Where(a => a.codCyber == cond.codCyber).FirstOrDefault();

                if (Cond == null)
                {
                    Cond = new CondicionesVenta();
                    Cond.codCyber = cond.codCyber;
   
                    Cond.Nombre = cond.Nombre;



                    db.CondicionesVenta.Add(Cond);
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Esta condicion de venta YA existe");
                }

                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK, Cond);
            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("api/CondicionesVenta/Actualizar")]
        public HttpResponseMessage Put([FromBody] CondicionesVenta cond)
        {
            try
            {
                G.AbrirConexionAPP(out db);


                var Cond = db.CondicionesVenta.Where(a => a.codCyber == cond.codCyber).FirstOrDefault();

                if (Cond != null)
                {
                    db.Entry(Cond).State = EntityState.Modified;
               
                    Cond.Nombre = cond.Nombre;

                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Condicion de venta no existe");
                }

                return Request.CreateResponse(HttpStatusCode.OK, Cond);
            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Route("api/CondicionesVenta/Eliminar")]
        public HttpResponseMessage Delete([FromUri] string id)
        {
            try
            {
                G.AbrirConexionAPP(out db);


                var Cond = db.CondicionesVenta.Where(a => a.codCyber == id).FirstOrDefault();

                if (Cond != null)
                {


                    db.CondicionesVenta.Remove(Cond);
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Condicion de venta no existe");
                }
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}