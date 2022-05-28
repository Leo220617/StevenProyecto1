using CheckIn.API.Models;
using CheckIn.API.Models.ModelCliente;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CheckIn.API.Controllers
{
    [Authorize]

    public class ParametrosController : ApiController
    {
        ModelCliente db;
        G G = new G();


        [Route("api/Parametros/Consultar")]
        public HttpResponseMessage GetOne([FromUri]int id)
        {
            try
            {
                G.AbrirConexionAPP(out db);



                var Parametros = db.Parametros.FirstOrDefault();


                if (Parametros == null)
                {
                    throw new Exception("Este parametro no se encuentra registrado");
                }

                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, Parametros);
            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("api/Parametros/Actualizar")]
        public HttpResponseMessage Put([FromBody] Parametros param)
        {
            try
            {
                G.AbrirConexionAPP(out db);


                var Parametros = db.Parametros.FirstOrDefault();

                if (Parametros != null)
                {
                    db.Entry(Parametros).State = EntityState.Modified;
    

                    Parametros.urlCyber = param.urlCyber;
                    Parametros.urlCyberRespHacienda = param.urlCyberRespHacienda;
                    Parametros.urlCyberAceptacion = param.urlCyberAceptacion;


                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Parametros no existe");
                }
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.OK, Parametros);
            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}