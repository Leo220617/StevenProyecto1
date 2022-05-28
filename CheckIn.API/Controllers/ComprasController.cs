using CheckIn.API.Models;
using CheckIn.API.Models.ModelCliente;
using CheckIn.API.Models.ModelMain;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;

namespace CheckIn.API.Controllers
{
    public class ComprasController: ApiController
    {
        ModelCliente db;
        G G = new G();

        [Route("api/Compras/RealizarLecturaEmail")]

        public async System.Threading.Tasks.Task<HttpResponseMessage> GetRealizarLecturaEmailsAsync()
        {
            try
            {
                G.AbrirConexionAPP(out db);
                var Parametros = db.Parametros.FirstOrDefault();
                var Correos = db.CorreosRecepcion.ToList();

                foreach (var item in Correos)
                {


                    using (ImapClient client = new ImapClient(item.RecepcionHostName, (int)(item.RecepcionPort),
                               item.RecepcionEmail, item.RecepcionPassword, AuthMethod.Login, (bool)(item.RecepcionUseSSL)))
                    {
                        IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());

                        DateTime recepcionUltimaLecturaImap = DateTime.Now;
                        if (item.RecepcionUltimaLecturaImap != null)
                            recepcionUltimaLecturaImap = item.RecepcionUltimaLecturaImap.Value;

                        uids.Concat(client.Search(SearchCondition.SentSince(recepcionUltimaLecturaImap)));

                        foreach (var uid in uids)
                        {
                            System.Net.Mail.MailMessage message = client.GetMessage(uid);

                            if (message.Attachments.Count > 0)
                            {
                                try
                                {
                                    byte[] ByteArrayPDF = null;
                                    int i = 1;

                                    decimal idGeneral = 0;
                                    foreach (var attachment in message.Attachments)
                                    {

                                        try
                                        {
                                            System.IO.StreamReader sr = new System.IO.StreamReader(attachment.ContentStream);



                                            string texto = sr.ReadToEnd();

                                            if (texto.Substring(0, 3) == "???")
                                                texto = texto.Substring(3);

                                            if (texto.Contains("PDF"))
                                            {

                                                ByteArrayPDF = ((MemoryStream)attachment.ContentStream).ToArray();
                                                //ByteArrayPDF = G.Zip(texto);


                                            }


                                            if (texto.Contains("FacturaElectronica")
                                                    || texto.Contains("NotaCreditoElectronica")
                                                    && !texto.Contains("TiqueteElectronico")

                                                    //  && !texto.Contains("NotaCreditoElectronica")
                                                    && !texto.Contains("NotaDebitoElectronica"))
                                            {
                                                var emailByteArray = G.Zip(texto);

                                                decimal id = db.Database.SqlQuery<decimal>("Insert Into BandejaEntrada(XmlFactura, Procesado, Asunto, Remitente,Pdf,impuestoAcreditar,gastoAplicable) " +
                                                        " VALUES (@EmailJson, 0, @Asunto, @Remitente, @Pdf,0,0); SELECT SCOPE_IDENTITY(); ",
                                                        new SqlParameter("@EmailJson", emailByteArray),
                                                        new SqlParameter("@Asunto", message.Subject),
                                                        new SqlParameter("@Remitente", message.From.ToString()),
                                                        new SqlParameter("@Pdf", (ByteArrayPDF == null ? new byte[0] : ByteArrayPDF))).First();
                                                idGeneral = id;
                                                try
                                                {

                                                    var datos = G.ObtenerDatosXmlRechazado(texto);
                                                    var Detalle = db.BandejaEntrada.Where(a => a.NumeroConsecutivo == datos.NumeroConsecutivo && a.IdEmisor == datos.Numero).FirstOrDefault();
                                                    if (datos.IdReceptor == db.Sucursales.FirstOrDefault().Cedula && Detalle == null && !string.IsNullOrEmpty(datos.NumeroConsecutivo))
                                                    {
                                                        db.Database.ExecuteSqlCommand("Update BandejaEntrada set NumeroConsecutivo=@NumeroConsecutivo, " +
                                                       " TipoDocumento = @TipoDocumento, FechaEmision = @FechaEmision , " +
                                                       " NombreEmisor = @NombreEmisor,IdEmisor = @IdEmisor ,CodigoMoneda = @CodigoMoneda , " +
                                                       " TotalComprobante = @TotalComprobante, " +
                                                       " Impuesto = @TotalImpuesto, " +
                                                       " tipoIdentificacionEmisor = @EmisorId," +
                                                       " IVA0 = @IVA0," +
                                                       " IVA1 = @IVA1," +
                                                       " IVA2 = @IVA2," +
                                                       " IVA4 = @IVA4," +
                                                       " IVA8 = @IVA8," +
                                                       " IVA13 = @IVA13" +


                                                       " WHERE Id=@Id ",
                                                        new SqlParameter("@NumeroConsecutivo", datos.NumeroConsecutivo),
                                                        new SqlParameter("@TipoDocumento", datos.TipoDocumento),
                                                        new SqlParameter("@FechaEmision", datos.FechaEmision),
                                                        new SqlParameter("@NombreEmisor", datos.NombreEmisor),
                                                        new SqlParameter("@IdEmisor", datos.Numero),
                                                        new SqlParameter("@CodigoMoneda", datos.CodigoMoneda),
                                                        new SqlParameter("@TotalComprobante", datos.TotalComprobante),
                                                        new SqlParameter("@Id", id),
                                                        new SqlParameter("@TotalImpuesto", datos.Impuesto),
                                                        new SqlParameter("@EmisorId", datos.tipoIdentificacionEmisor),

                                                        new SqlParameter("@IVA0", datos.IVA0),
                                                        new SqlParameter("@IVA1", datos.IVA1),
                                                        new SqlParameter("@IVA2", datos.IVA2),                                                                                                                
                                                        new SqlParameter("@IVA4", datos.IVA4),
                                                        new SqlParameter("@IVA8", datos.IVA8),
                                                        new SqlParameter("@IVA13", datos.IVA13));


                                                       
                                                        
                                                    }
                                                    else
                                                    {

                                                        db.Database.ExecuteSqlCommand("DELETE FROM BANDEJAENTRADA where Id=" + id);
                                                        throw new Exception("Este documento no es para este usuario o ya esta registrado");
                                                    }


                                                }
                                                catch(Exception ex) {

                                                    BitacoraErrores be = new BitacoraErrores();
                                                    be.Descripcion = ex.Message;
                                                    be.StackTrace = ex.StackTrace;
                                                    be.Fecha = DateTime.Now;
                                                    be.DocNum = id.ToString();
                                                    db.BitacoraErrores.Add(be);
                                                    db.SaveChanges();

                                                }
                                            }

                                            if (i == message.Attachments.Count())
                                            {
                                                if (idGeneral > 0)
                                                {
                                                    var bandeja = db.BandejaEntrada.Where(a => a.Id == idGeneral).FirstOrDefault();

                                                    if (bandeja.Pdf.Count() == 0)
                                                    {
                                                        db.Database.ExecuteSqlCommand("Update BandejaEntrada set Pdf=@Pdf " +

                                                   " WHERE Id=@Id ",
                                                    new SqlParameter("@Pdf", ByteArrayPDF),

                                                    new SqlParameter("@Id", idGeneral));
                                                    }
                                                    bandeja = db.BandejaEntrada.Where(a => a.Id == idGeneral).FirstOrDefault();
                                                    db.Entry(bandeja).State = EntityState.Modified;
                                                    bandeja.impuestoAcreditar = 0;
                                                    bandeja.gastoAplicable = 0;
                                                    bandeja.CodigoActividad = db.Sucursales.FirstOrDefault().CodActividadComercial;
                                                    bandeja.XmlConfirmacion = G.GuardarPDF(ByteArrayPDF, bandeja.NumeroConsecutivo + "_" + bandeja.NombreEmisor);
                                                    db.SaveChanges();

                                                }
                                            }

                                            i++;
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
                            }
                            message.Dispose();

                            await System.Threading.Tasks.Task.Delay(100);
                        }
                        db.Entry(item).State = EntityState.Modified;
                        item.RecepcionUltimaLecturaImap = DateTime.Now;
                        db.SaveChanges();

                    }

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


        [Route("api/Compras/Listado")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetListado([FromUri] Filtros filtro)
        {
            try
            {
                G.AbrirConexionAPP(out db);

                DateTime time = new DateTime();


                var Compras = db.BandejaEntrada.Where(a => (filtro.FechaInicio != time ? a.FechaIngreso >= filtro.FechaFinal : true)).ToList();

                if (filtro.FechaFinal != time)
                {
                    filtro.FechaFinal = filtro.FechaFinal.AddDays(1);
                    Compras = Compras.Where(a => a.FechaIngreso <= filtro.FechaFinal).ToList();
                }
                G.CerrarConexionAPP(db);


                return Request.CreateResponse(HttpStatusCode.OK, Compras);

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }




    }
}