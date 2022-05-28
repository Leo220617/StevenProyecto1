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
    public class UbicacionesController : ApiController
    {
        ModelCliente db;
        G G = new G();

   
        [Route("api/Ubicaciones/Cantones")]
        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Cantones = db.Cantones.ToList();

                if(filtro.Codigo1 > 0)
                {
                    Cantones = Cantones.Where(a => a.CodProvincia == filtro.Codigo1).ToList();
                }


                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, Cantones);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/Ubicaciones/Distritos")]
        public async Task<HttpResponseMessage> GetDis([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Distritos = db.Distritos.ToList();

                if (filtro.Codigo1 > 0)
                {
                    Distritos = Distritos.Where(a => a.CodProvincia == filtro.Codigo1).ToList();
                }

                if(filtro.Codigo2 > 0)
                {
                    Distritos = Distritos.Where(a => a.CodCanton == filtro.Codigo2).ToList();

                }

                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, Distritos);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/Ubicaciones/Barrios")]
        public async Task<HttpResponseMessage> GetBarrios([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Barrios = db.Barrios.ToList();

                if (filtro.Codigo1 > 0)
                {
                    Barrios = Barrios.Where(a => a.CodProvincia == filtro.Codigo1).ToList();
                }

                if (filtro.Codigo2 > 0)
                {
                    Barrios = Barrios.Where(a => a.CodCanton == filtro.Codigo2).ToList();

                }

                if (filtro.Codigo3 > 0)
                {
                    Barrios = Barrios.Where(a => a.CodDistrito == filtro.Codigo3).ToList();

                }

                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, Barrios);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}