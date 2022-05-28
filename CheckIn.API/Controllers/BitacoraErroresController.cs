using CheckIn.API.Models;
using CheckIn.API.Models.ModelCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace CheckIn.API.Controllers
{
    [Authorize]
    public class BitacoraErroresController : ApiController
    {
        ModelCliente db;
        G G = new G();

        
        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var time = new DateTime();
                var BE = db.BitacoraErrores.Where(a => (filtro.FechaInicio != time ? a.Fecha >= filtro.FechaInicio : true) && (filtro.FechaFinal != time ? a.Fecha <= filtro.FechaFinal : true)).ToList();

               

                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, BE);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}