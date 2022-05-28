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

    public class ClientesController: ApiController
    {
        ModelCliente db;
        G G = new G();

        public async Task<HttpResponseMessage> Get([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Clientes = db.Clientes.ToList();

                if (!string.IsNullOrEmpty(filtro.Texto))
                {
                    Clientes = Clientes.Where(a => a.Nombre.ToUpper().Contains(filtro.Texto.ToUpper()) ).ToList();
                }


                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, Clientes);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Route("api/Clientes/Consultar")]
        public async Task<HttpResponseMessage> GetOne([FromUri] int id)
        {
            try
            {
                G.AbrirConexionAPP(out db);
                var Clientes = db.Clientes.Where(a => a.id == id).FirstOrDefault();

                if (Clientes == null)
                {
                    throw new Exception("Cliente no existe");
                }


                G.CerrarConexionAPP(db);
                return Request.CreateResponse(HttpStatusCode.OK, Clientes);

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
        public HttpResponseMessage Post([FromBody] Clientes cliente)
        {

            G.AbrirConexionAPP(out db);
            var t = db.Database.BeginTransaction();
           
            try
            {
                var Cliente = db.Clientes.Where(a => a.id == cliente.id).FirstOrDefault();
               if(Cliente == null)
                {
                    Cliente = new Clientes();
                    Cliente.Nombre = cliente.Nombre;
                    Cliente.Cedula = cliente.Cedula;
                    Cliente.TipoCedula = cliente.TipoCedula;
                    Cliente.Email = cliente.Email;
                    Cliente.CodPais = "506";
                    Cliente.Telefono = cliente.Telefono;
                    Cliente.provincia = cliente.provincia;
                    Cliente.canton = cliente.canton;
                    Cliente.distrito = cliente.distrito;
                    Cliente.barrio = cliente.barrio;
                    Cliente.sennas = cliente.sennas;
                    db.Clientes.Add(Cliente);
                    db.SaveChanges();

                    t.Commit();
                }
                else
                {
                    throw new Exception("Este cliente YA existe");
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
        [Route("api/Clientes/Actualizar")]
        public HttpResponseMessage Put([FromBody] Clientes cliente)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                
                var Cliente = db.Clientes.Where(a => a.id == cliente.id).FirstOrDefault();

                if (Cliente != null)
                {
                    db.Entry(Cliente).State = System.Data.Entity.EntityState.Modified;
                    Cliente.Nombre = cliente.Nombre;
                    Cliente.Cedula = cliente.Cedula;
                    Cliente.TipoCedula = cliente.TipoCedula;
                    Cliente.Email = cliente.Email;
                    Cliente.CodPais = "506";
                    Cliente.Telefono = cliente.Telefono;
                    Cliente.provincia = cliente.provincia;
                    Cliente.canton = cliente.canton;
                    Cliente.distrito = cliente.distrito;
                    Cliente.barrio = cliente.barrio;
                    Cliente.sennas = cliente.sennas;
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Cliente no existe");
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
        [Route("api/Clientes/Eliminar")]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Cliente = db.Clientes.Where(a => a.id == id).FirstOrDefault();


                if (Cliente != null )
                {
                    db.Clientes.Remove(Cliente);
                   
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Cliente no existe");
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