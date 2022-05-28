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

    public class ProductosController: ApiController
    {
        ModelCliente db;
        G G = new G();


        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Productos = db.Productos.ToList();

                if (!string.IsNullOrEmpty(filtro.Texto))
                {
                    Productos = Productos.Where(a => a.Nombre.ToUpper().Contains(filtro.Texto.ToUpper())).ToList();
                }


                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, Productos);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [Route("api/Productos/Consultar")]
        public async Task<HttpResponseMessage> GetOne([FromUri] int id)
        {
            try
            {
                G.AbrirConexionAPP(out db);
                var Productos = db.Productos.Where(a => a.id == id).FirstOrDefault();

                if (Productos == null)
                {
                    throw new Exception("Producto no existe");
                }


                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, Productos);

            }
            catch (Exception ex)
            {
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpPost]
        public HttpResponseMessage Post([FromBody] Productos producto)
        {

            G.AbrirConexionAPP(out db);
            var t = db.Database.BeginTransaction();

            try
            {
                var Producto = db.Productos.Where(a => a.id == producto.id).FirstOrDefault();
                if (Producto == null)
                {
                    Producto = new Productos();
                    Producto.TipoCodigo = producto.TipoCodigo;
                    Producto.Codigo = producto.Codigo;
                    Producto.UnidadMedida = producto.UnidadMedida;
                    Producto.Nombre = producto.Nombre;
                    Producto.PrecioUnitario = producto.PrecioUnitario;
                    Producto.Impuesto = producto.Impuesto;
                    Producto.Cabys = producto.Cabys;
                    db.Productos.Add(Producto);
                    db.SaveChanges();

                    t.Commit();
                }
                else
                {
                    throw new Exception("Este Producto YA existe");
                }

                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                t.Rollback();
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();


                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("api/Productos/Actualizar")]
        public HttpResponseMessage Put([FromBody] Productos producto)
        {
            try
            {
                G.AbrirConexionAPP(out db);


                var Producto = db.Productos.Where(a => a.id == producto.id).FirstOrDefault();


                if (Producto != null)
                {
                    db.Entry(Producto).State = System.Data.Entity.EntityState.Modified;
                    Producto.TipoCodigo = producto.TipoCodigo;
                    Producto.Codigo = producto.Codigo;
                    Producto.UnidadMedida = producto.UnidadMedida;
                    Producto.Nombre = producto.Nombre;
                    Producto.PrecioUnitario = producto.PrecioUnitario;
                    Producto.Impuesto = producto.Impuesto;
                    Producto.Cabys = producto.Cabys;
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Producto no existe");
                }
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();

                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpDelete]
        [Route("api/Productos/Eliminar")]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Producto = db.Productos.Where(a => a.id == id).FirstOrDefault();


                if (Producto != null)
                {
                    db.Productos.Remove(Producto);

                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Producto no existe");
                }
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {


                BitacoraErrores be = new BitacoraErrores();
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