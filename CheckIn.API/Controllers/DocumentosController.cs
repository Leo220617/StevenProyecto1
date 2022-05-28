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

    public class DocumentosController : ApiController
    {
        ModelCliente db;
        G G = new G();
        Metodos metodo = new Metodos();


        [HttpPost]
        public async System.Threading.Tasks.Task<HttpResponseMessage> PostAsync([FromBody] EncDocumentoViewModel enc)
        {
            G.AbrirConexionAPP(out db);

            var t = db.Database.BeginTransaction();
            try
            {
                Parametros parametros = db.Parametros.FirstOrDefault();
                EncDocumento documento = new EncDocumento();
               

                
                var Sucursal = db.Sucursales.Where(a => a.codSuc == enc.idSucursal).FirstOrDefault();
                var consecInterno = 0;

                switch(enc.TipoDocumento)
                {
                    case "01":
                        {
                            consecInterno = Sucursal.consecFac.Value;
                            db.Entry(Sucursal).State = System.Data.Entity.EntityState.Modified;
                            Sucursal.consecFac++;
                            db.SaveChanges();
                            break;
                        }
                    case "02":
                        {
                            consecInterno = Sucursal.consecND.Value;
                            db.Entry(Sucursal).State = System.Data.Entity.EntityState.Modified;
                            Sucursal.consecND++;
                            db.SaveChanges();
                            break;
                        }
                    case "03":
                        {
                            consecInterno = Sucursal.consecNC.Value;
                            db.Entry(Sucursal).State = System.Data.Entity.EntityState.Modified;
                            Sucursal.consecNC++;
                            db.SaveChanges();
                            break;
                        }
                    case "04":
                        {
                            consecInterno = Sucursal.consecTiq.Value;
                            db.Entry(Sucursal).State = System.Data.Entity.EntityState.Modified;
                            Sucursal.consecTiq++;
                            db.SaveChanges();
                            break;
                        }
                    case "08":
                        {
                            consecInterno = Sucursal.consecFEC.Value;
                            db.Entry(Sucursal).State = System.Data.Entity.EntityState.Modified;
                            Sucursal.consecFEC++;
                            db.SaveChanges();
                            break;
                        }
                }



                documento.TipoDocumento = enc.TipoDocumento; //Tipo de documento a generar 
                documento.Fecha = DateTime.Now; // Se toma la fecha del comprobante y se le pone la hora actual
                documento.idSucursal = enc.idSucursal;
                documento.CodActividadEconomica = enc.CodActividadEconomica;
                documento.consecutivoInterno = consecInterno;

                 var Cliente = db.Clientes.Where(a => a.id == enc.CodCliente).FirstOrDefault();

                documento.CodCliente = enc.CodCliente;
                documento.NombreCliente = Cliente.Nombre;
                documento.Email = Cliente.Email;
                documento.TipoIdentificacion = Cliente.TipoCedula;
                documento.CedulaCliente = Cliente.Cedula;
                documento.condicionVenta = enc.condicionVenta;
                documento.plazoCredito = enc.plazoCredito;
                documento.medioPago = enc.medioPago;
                documento.moneda = enc.moneda;
                documento.tipoCambio = enc.tipoCambio;
                documento.RefNumeroDocumento = enc.RefNumeroDocumento;
                documento.RefRazon = enc.RefRazon;
                documento.RefFechaEmision = DateTime.Now;
                documento.procesadaHacienda = false;



                db.EncDocumento.Add(documento);
                db.SaveChanges();

                    try
                    {
                        if (!string.IsNullOrEmpty(enc.OtroTexto))
                        {
                            OtrosTextos ot = new OtrosTextos();
                            ot.idEncabezado = enc.id;
                            ot.codigo = "99";
                            ot.detalle = enc.OtroTexto;
                            db.OtrosTextos.Add(ot);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {

                        BitacoraErrores be = new BitacoraErrores();
                        be.DocNum = enc.id.ToString();
                        be.Type = enc.TipoDocumento;
                        be.Descripcion = ex.Message;
                        be.StackTrace = ex.StackTrace;
                        be.Fecha = DateTime.Now;
                        db.BitacoraErrores.Add(be);
                        db.SaveChanges();
                    }

                 


                    List<DetDocumento> Detalles = new List<DetDocumento>();
 

                    decimal totalserviciosexonerado = 0;
                    decimal totalmercaderiasexoneradas = 0;
                    decimal totalsergravados = 0;
                    decimal totalmercaderiagravada = 0;
                    decimal totalservexentos = 0;
                    decimal totalmercexenta = 0;


                    var i = 1;
                    foreach (var item in enc.DetDocumento)
                    {
                        DetDocumento det = new DetDocumento();
                        det.idEncabezado = documento.id;
                        det.NumLinea = i;//Convert.ToInt32(item["NumLinea"]);
                        if (documento.TipoDocumento == "09")
                        {
                            det.partidaArancelaria = item.partidaArancelaria;
                            var exp = item.idImpuesto;
                            var ImpuestoE = db.Impuestos.Where(a => a.id == exp).FirstOrDefault();
                            det.exportacion = Math.Round(Convert.ToDecimal(det.SubTotal * (ImpuestoE.tarifa / 100)), 2);
                        }
                        else
                        {
                            det.partidaArancelaria = item.partidaArancelaria;
                            det.exportacion = 0;
                            det.partidaArancelaria = det.exportacion == 0 ? "" : det.partidaArancelaria;
                        }

                        det.CodCabys = item.CodCabys;
                        det.tipoCod = item.tipoCod;
                        det.codPro = item.codPro;
                        det.cantidad = Convert.ToDecimal(item.cantidad) == 0 ? 1 : Convert.ToDecimal(item.cantidad);
                        var unidMedida = Convert.ToInt32( item.unidadMedida);
                        det.unidadMedida = db.UnidadesMedida.Where(a => a.id == unidMedida).FirstOrDefault().codCyber;
                        var unidMedidaC = Convert.ToInt32(item.unidadMedidaComercial);
                        det.unidadMedidaComercial = db.UnidadesMedida.Where(a => a.id == unidMedidaC).FirstOrDefault().Nombre;
                        det.NomPro = item.NomPro;
                        det.PrecioUnitario = Math.Round(Convert.ToDecimal(item.PrecioUnitario), 2);
                        det.MontoTotal = Math.Round(det.cantidad.Value * det.PrecioUnitario.Value, 2);
                        var desc = Convert.ToDecimal(item.PorDesc) / 100;
                        det.MontoDescuento = det.MontoTotal * desc < 0 ? 0 : Math.Round(det.MontoTotal.Value * desc, 2);
                        det.NaturalezaDescuento = string.IsNullOrEmpty(item.NaturalezaDescuento.ToString()) ? "Descuento" : item.NaturalezaDescuento.ToString();
                        det.SubTotal = Math.Round(det.MontoTotal.Value - det.MontoDescuento.Value, 2);
                        det.idImpuesto = item.idImpuesto.ToString();
                        det.factorIVA = Convert.ToDecimal(item.factorIVA);
                        det.baseImponible = Math.Round(det.SubTotal.Value, 2);
                        var Impuesto = db.Impuestos.Where(a => a.id == det.idImpuesto).FirstOrDefault();
                        det.montoImpuesto = Math.Round(Convert.ToDecimal(det.SubTotal * (Impuesto.tarifa / 100)), 2);

                        

                        if (!string.IsNullOrEmpty(item.exonNumdoc.ToString()))
                        {
                            if (Convert.ToInt32(item.exonNumdoc) > 0)
                            {
                                try
                                {
                                     

                                    det.exonTipoDoc = item.exonTipoDoc;
                                    det.exonNumdoc = item.exonNumdoc;
                                    det.exonNomInst = item.exonNomInst;
                                det.exonFecEmi = item.exonFecEmi;
                                    
                                    det.exonPorExo = item.exonPorExo; // Convert.ToInt32(db.Impuestos.Where(a => a.id == tipoImp).FirstOrDefault().tarifa.Value);

                                    det.exonMonExo = Math.Round((det.SubTotal.Value * det.exonPorExo.Value / 100), 2);

                                    
                                    decimal total = 0;
                                    if (det.unidadMedida == "Sp")
                                    {
                                        if (Impuesto.tarifa > 0)
                                        {
                                            if (det.exonPorExo > 0)
                                            {
                                                if (Impuesto.tarifa - det.exonPorExo < 0)
                                                {
                                                    totalserviciosexonerado += det.MontoTotal.Value;
                                                    total = det.MontoTotal.Value;
                                                }
                                                else
                                                {
                                                    totalserviciosexonerado += Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                                    total = Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                                }


                                                totalsergravados += det.MontoTotal.Value + det.MontoDescuento.Value - total;
                                            }
                                            else
                                            {
                                                totalsergravados += (1 - (det.exonPorExo.Value / 100)) * det.MontoTotal.Value;
                                            }
                                        }
                                        else
                                        {
                                            totalservexentos += det.MontoTotal.Value;
                                        }
                                    }
                                    else
                                    {
                                        if (Impuesto.tarifa > 0)
                                        {
                                            if (det.exonPorExo > 0)
                                            {
                                                if (Impuesto.tarifa - det.exonPorExo < 0)
                                                {
                                                    totalmercaderiasexoneradas += det.MontoTotal.Value;
                                                    total = det.MontoTotal.Value;
                                                }
                                                else
                                                {
                                                    totalmercaderiasexoneradas += Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                                    total = Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                                }

                                                totalmercaderiagravada += det.MontoTotal.Value - total;
                                            }
                                            else
                                            {
                                                totalmercaderiagravada += (1 - (det.exonPorExo.Value / 100)) * det.MontoTotal.Value;
                                            }
                                        }
                                        else
                                        {
                                            totalmercexenta += det.MontoTotal.Value;
                                        }
                                    }


                                }
                                catch (Exception ex1)
                                {
                                    det.exonFecEmi = DateTime.Now;
                                    det.exonMonExo = 0;

                                }

                            }
                            else
                            {
                                det.exonFecEmi = DateTime.Now;
                                det.exonMonExo = 0;

                                decimal total = 0;
                                if (det.unidadMedida == "Sp")
                                {
                                    if (Impuesto.tarifa > 0)
                                    {
                                        if (det.exonPorExo > 0)
                                        {
                                            if (Impuesto.tarifa - det.exonPorExo < 0)
                                            {
                                                totalserviciosexonerado += det.MontoTotal.Value;
                                                total = det.MontoTotal.Value;
                                            }
                                            else
                                            {
                                                totalserviciosexonerado += Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                                total = Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                            }


                                            totalsergravados += det.MontoTotal.Value + det.MontoDescuento.Value - total;
                                        }
                                        else
                                        {
                                            totalsergravados += (1 - (det.exonPorExo.Value / 100)) * det.MontoTotal.Value;
                                        }
                                    }
                                    else
                                    {
                                        totalservexentos += det.MontoTotal.Value;
                                    }
                                }
                                else
                                {
                                    if (Impuesto.tarifa > 0)
                                    {
                                        if (det.exonPorExo > 0)
                                        {
                                            if (Impuesto.tarifa - det.exonPorExo < 0)
                                            {
                                                totalmercaderiasexoneradas += det.MontoTotal.Value;
                                                total = det.MontoTotal.Value;
                                            }
                                            else
                                            {
                                                totalmercaderiasexoneradas += Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                                total = Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                            }

                                            totalmercaderiagravada += det.MontoTotal.Value - total;
                                        }
                                        else
                                        {
                                            totalmercaderiagravada += (1 - (det.exonPorExo.Value / 100)) * det.MontoTotal.Value;
                                        }
                                    }
                                    else
                                    {
                                        totalmercexenta += det.MontoTotal.Value;
                                    }
                                }

                            }
                        }
                        else
                        {
                            det.exonFecEmi = DateTime.Now;
                            det.exonMonExo = 0;
                            det.exonPorExo = 0;
                            decimal total = 0;
                            if (det.unidadMedida == "Sp")
                            {
                                if (Impuesto.tarifa > 0)
                                {
                                    if (det.exonPorExo > 0)
                                    {
                                        if (Impuesto.tarifa - det.exonPorExo < 0)
                                        {
                                            totalserviciosexonerado += det.MontoTotal.Value;
                                            total = det.MontoTotal.Value;
                                        }
                                        else
                                        {
                                            totalserviciosexonerado += Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                            total = Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                        }


                                        totalsergravados += det.MontoTotal.Value + det.MontoDescuento.Value - total;
                                    }
                                    else
                                    {
                                        totalsergravados += (1 - (det.exonPorExo.Value / 100)) * det.MontoTotal.Value;
                                    }
                                }
                                else
                                {
                                    totalservexentos += det.MontoTotal.Value;
                                }
                            }
                            else
                            {
                                if (Impuesto.tarifa > 0)
                                {
                                    if (det.exonPorExo > 0)
                                    {
                                        if (Impuesto.tarifa - det.exonPorExo < 0)
                                        {
                                            totalmercaderiasexoneradas += det.MontoTotal.Value;
                                            total = det.MontoTotal.Value;
                                        }
                                        else
                                        {
                                            totalmercaderiasexoneradas += Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                            total = Convert.ToDecimal(det.MontoTotal * (det.exonPorExo / Impuesto.tarifa));
                                        }

                                        totalmercaderiagravada += det.MontoTotal.Value - total;
                                    }
                                    else
                                    {
                                        totalmercaderiagravada += (1 - (det.exonPorExo.Value / 100)) * det.MontoTotal.Value;
                                    }
                                }
                                else
                                {
                                    totalmercexenta += det.MontoTotal.Value;
                                }
                            }

                        }



                        det.impNeto = det.montoImpuesto - det.exonMonExo;
                        det.totalLinea = det.SubTotal + det.impNeto;

                        db.DetDocumento.Add(det);
                        db.SaveChanges();
                        Detalles.Add(det);
                        i++;
                    }

                    db.Entry(documento).State = System.Data.Entity.EntityState.Modified;
                    // enc.totalserviciogravado = Math.Round(Detalles.Where(a => a.unidadMedida.ToLower() == "sp" && a.exonTipoDoc == null).Sum(d => d.MontoTotal), 2);
                    //enc.totalservicioexento = Math.Round(Detalles.Where(a => a.unidadMedida.ToLower() == "sp" && a.exonTipoDoc != null).Sum(d => d.MontoTotal), 2);
                    //enc.totalservicioexonerado = Math.Round(totalserviciosexonerado, 2);

                    documento.totalserviciogravado = Math.Round(totalsergravados, 2);
                documento.totalservicioexento = Math.Round(totalservexentos, 2);
                documento.totalservicioexonerado = Math.Round(totalserviciosexonerado, 2);



                // enc.totalmercaderiagravado = Math.Round(Detalles.Where(a => a.unidadMedida.ToLower() != "sp" && a.exonTipoDoc == null).Sum(d => d.MontoTotal), 2);
                //enc.totalmercaderiaexenta = Math.Round(Detalles.Where(a => a.unidadMedida.ToLower() != "sp" && a.exonTipoDoc != null).Sum(d => d.MontoTotal), 2);
                //enc.totalmercaderiaexonerado = Math.Round(totalmercaderiasexoneradas, 2);

                documento.totalmercaderiagravado = Math.Round(totalmercaderiagravada, 2);
                documento.totalmercaderiaexenta = Math.Round(totalmercexenta, 2);
                documento.totalmercaderiaexonerado = Math.Round(totalmercaderiasexoneradas, 2);


                documento.totalgravado = Math.Round((documento.totalserviciogravado + documento.totalmercaderiagravado).Value, 2);
                documento.totalexento = Math.Round((documento.totalservicioexento + documento.totalmercaderiaexenta).Value, 2);
                documento.totalexonerado = Math.Round((documento.totalservicioexonerado + documento.totalmercaderiaexonerado).Value, 2);

                documento.totalventa = Math.Round((documento.totalgravado + documento.totalexento + documento.totalexonerado).Value, 2);
                documento.totaldescuentos = Math.Round(Detalles.Sum(a => a.MontoDescuento).Value, 2);
                documento.totalventaneta = Math.Round((documento.totalventa - documento.totaldescuentos).Value, 2);
                documento.totalimpuestos = Math.Round(Detalles.Sum(a => a.impNeto).Value, 2);
                documento.totalivadevuelto = 0; //Servicios de salud no aplicables
                documento.totalotroscargos = Math.Round(db.OtrosCargos.Where(a => a.idEncabezado == documento.id).FirstOrDefault() == null ? 0 : db.OtrosCargos.Where(a => a.idEncabezado == documento.id).Sum(d => d.monto).Value, 2);
                documento.montoOtrosCargos = Math.Round(documento.totalotroscargos.Value, 2);
                documento.totalcomprobante = Math.Round((documento.totalventaneta + documento.totalimpuestos + documento.totalotroscargos - documento.totalivadevuelto).Value, 2);

                    if (documento.TipoDocumento == "03") //Si es nota de credito
                    {
                    var referencia = Convert.ToInt32(enc.RefNumeroDocumento);
                        var Encabezado = db.EncDocumento.Where(a => a.consecutivoInterno == referencia && !string.IsNullOrEmpty(a.ClaveHacienda)).FirstOrDefault();

                        if (Encabezado != null)
                        {
                        documento.RefTipoDocumento = Encabezado.TipoDocumento;
                        documento.RefFechaEmision = Encabezado.Fecha.Value;

                            try
                            {
                                if (string.IsNullOrEmpty(enc.RefRazon))
                                {

                                    if (Math.Abs((decimal)(documento.totalcomprobante - Encabezado.totalcomprobante)) < 1)
                                    {
                                    documento.RefCodigo = "01";
                                    documento.RefRazon = $"Anula documento electrónico { Encabezado.ClaveHacienda}";
                                    }
                                    else
                                    {
                                    documento.RefCodigo = "02";
                                    documento.RefRazon = $"Corrige monto documento electrónico { Encabezado.ClaveHacienda}";
                                    }
                                }
                                else
                                {
                                    if (Math.Abs((decimal)(documento.totalcomprobante - Encabezado.totalcomprobante)) < 1)
                                    {
                                    documento.RefCodigo = "01";

                                    }
                                    else
                                    {
                                    documento.RefCodigo = "02";

                                    }
                                }

                            documento.RefNumeroDocumento = Encabezado.ClaveHacienda;



                            }
                            catch (Exception pp)
                            {
                            documento.RefCodigo = "01";
                            documento.RefRazon = $"Anula documento electrónico { Encabezado.ClaveHacienda}";

                            }
                        }
                        else
                        {
                            
                        }


                    }



                    if (documento.TipoDocumento == "02") //Si es nota de debito
                    {
                    var referencia = Convert.ToInt32(enc.RefNumeroDocumento);

                    var Encabezado = db.EncDocumento.Where(a => a.consecutivoInterno == referencia && !string.IsNullOrEmpty(a.ClaveHacienda)).FirstOrDefault();

                        if (Encabezado != null)
                        {
                        documento.RefTipoDocumento = Encabezado.TipoDocumento;
                        documento.RefFechaEmision = Encabezado.Fecha.Value;

                            try
                            {
                                if (string.IsNullOrEmpty(documento.RefRazon))
                                {

                                    if (Math.Abs((decimal)(documento.totalcomprobante - Encabezado.totalcomprobante)) < 1)
                                    {
                                    documento.RefCodigo = "01";
                                    documento.RefRazon = $"Anula documento electrónico { Encabezado.ClaveHacienda}";
                                    }
                                    else
                                    {
                                    documento.RefCodigo = "02";
                                    documento.RefRazon = $"Corrige monto documento electrónico { Encabezado.ClaveHacienda}";
                                    }
                                }
                                else
                                {
                                    if (Math.Abs((decimal)(documento.totalcomprobante - Encabezado.totalcomprobante)) < 1)
                                    {
                                    documento.RefCodigo = "01";

                                    }
                                    else
                                    {
                                    documento.RefCodigo = "02";

                                    }
                                }

                            documento.RefNumeroDocumento = Encabezado.ClaveHacienda;

                            }
                            catch (Exception pp)
                            {
                            documento.RefCodigo = "01";
                            documento.RefRazon = $"Anula documento electrónico { Encabezado.ClaveHacienda}";

                            }
                        }
                        else
                        {
                             
                        }




                    }

                    db.SaveChanges();
                    t.Commit();

                    //Crear el xml a enviar
                    MakeXML xml = metodo.RellenarXML(documento, Detalles.ToArray());
                    HttpClient cliente = new HttpClient();

                    var httpContent = new StringContent(JsonConvert.SerializeObject(xml), Encoding.UTF8, "application/json");
                    cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    try
                    {
                        HttpResponseMessage response = await cliente.PostAsync(parametros.urlCyber, httpContent);

                        if (response.IsSuccessStatusCode)
                        {
                            response.Content.Headers.ContentType.MediaType = "application/json";
                            var resp = await response.Content.ReadAsAsync<respuesta>();



                            db.Entry(documento).State = System.Data.Entity.EntityState.Modified;
                            documento.procesadaHacienda = true;
                            documento.code = resp.code;
                            documento.RespuestaHacienda = resp.hacienda_mensaje;
                            documento.XMLFirmado = resp.data;
                            documento.ClaveHacienda = resp.clave;
                            documento.JSON = JsonConvert.SerializeObject(xml);
                            if (resp.clave != null)
                            {
                                if (resp.clave.Length > 3)
                                {
                                    documento.ConsecutivoHacienda = enc.ClaveHacienda.Substring(21, 20);



                                }
                            }
                            documento.ErrorCyber = resp.xml_error;

                            if (documento.code == 1)
                            {
                                //REspuesta de hacienda
                                cuerpoRespuesta cuerpo = new cuerpoRespuesta();
                                cuerpo.api_key = Sucursal.ApiKey;
                                cuerpo.clave = enc.ClaveHacienda;
                                HttpClient cliente2 = new HttpClient();

                                var httpContent2 = new StringContent(JsonConvert.SerializeObject(cuerpo), Encoding.UTF8, "application/json");
                                cliente2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                try
                                {
                                    HttpResponseMessage response2 = await cliente2.PostAsync(parametros.urlCyberRespHacienda, httpContent2);
                                    if (response2.IsSuccessStatusCode)
                                    {
                                        response2.Content.Headers.ContentType.MediaType = "application/json";
                                        var resp2 = await response2.Content.ReadAsAsync<respuestaHacienda>();

                                        if (resp2.data.ind_estado.Contains("aceptado"))
                                        {
                                            documento.RespuestaHacienda = resp2.data.ind_estado;
                                            documento.XMLFirmado = resp2.data.respuesta_xml;




                                        }
                                        else
                                        {
                                            documento.RespuestaHacienda = resp2.data.ind_estado;


                                        }
                                    }
                                }
                                catch (Exception ex)
                                {


                                }

                           
                            }



                            db.SaveChanges();
          

                        }

                    }
                    catch (Exception ex)
                    {
                        BitacoraErrores be = new BitacoraErrores();
                        be.DocNum = consecInterno.ToString();
                        be.Type = enc.TipoDocumento;
                        be.Descripcion = ex.Message;
                        be.StackTrace = ex.StackTrace;
                        be.Fecha = DateTime.Now;
                        db.BitacoraErrores.Add(be);
                        db.SaveChanges();
                    }





                G.CerrarConexionAPP(db);






                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                try
                {
                    t.Rollback();
                }
                catch (Exception x)
                {


                }
                BitacoraErrores be = new BitacoraErrores();
                be.DocNum = "";
                be.Type = enc.TipoDocumento;
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }


        [Route("api/Documentos/Consultar")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetOneAsync([FromUri] string Clave, string CodSucursal = "001")
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Sucursal = db.Sucursales.Where(a => a.codSuc == CodSucursal).FirstOrDefault();
                var parametros = db.Parametros.FirstOrDefault();
                //REspuesta de hacienda
                cuerpoRespuesta cuerpo = new cuerpoRespuesta();


                cuerpo.api_key = Sucursal.ApiKey;
                cuerpo.clave = Clave;
                HttpClient cliente2 = new HttpClient();

                var httpContent2 = new StringContent(JsonConvert.SerializeObject(cuerpo), Encoding.UTF8, "application/json");
                cliente2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response2 = await cliente2.PostAsync(parametros.urlCyberRespHacienda, httpContent2);
                    if (response2.IsSuccessStatusCode)
                    {
                        response2.Content.Headers.ContentType.MediaType = "application/json";
                        var resp2 = await response2.Content.ReadAsAsync<respuestaHacienda>();

                        var Documentos = db.EncDocumento.Where(a => a.ClaveHacienda == Clave).FirstOrDefault();
                        if(Documentos != null)
                        {
                            db.Entry(Documentos).State = System.Data.Entity.EntityState.Modified;
                            Documentos.RespuestaHacienda = resp2.data.ind_estado;
                            Documentos.ConsecutivoHacienda = Documentos.ClaveHacienda.Substring(21, 20);
                            db.SaveChanges();
                        }


                        G.CerrarConexionAPP(db);

                        return Request.CreateResponse(HttpStatusCode.OK, resp2);
                    }
                    else
                    {
                        G.CerrarConexionAPP(db);

                        return Request.CreateResponse(HttpStatusCode.OK, response2.IsSuccessStatusCode);
                    }
                }
                catch (Exception ex)
                {
                    G.CerrarConexionAPP(db);

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

                }

                //

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }

        [Route("api/Documentos/ConsultarAceptacion")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> GetOneAceptacionAsync([FromUri] string Clave, string CodSucursal = "001")
        {
            try
            {
                G.AbrirConexionAPP(out db);

                var Sucursal = db.Sucursales.Where(a => a.codSuc == CodSucursal).FirstOrDefault();
                var parametros = db.Parametros.FirstOrDefault();
                //REspuesta de hacienda
                cuerpoRespuesta cuerpo = new cuerpoRespuesta();


                cuerpo.api_key = Sucursal.ApiKey;
                cuerpo.clave = Clave;
                HttpClient cliente2 = new HttpClient();

                var httpContent2 = new StringContent(JsonConvert.SerializeObject(cuerpo), Encoding.UTF8, "application/json");
                cliente2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    HttpResponseMessage response2 = await cliente2.PostAsync(parametros.urlCyberConsultaHacienda, httpContent2);
                    if (response2.IsSuccessStatusCode)
                    {
                        response2.Content.Headers.ContentType.MediaType = "application/json";
                        try
                        {
                            var resp2 = await response2.Content.ReadAsAsync<respuestaHaciendaBandeja>();

                            var Documentos = db.BandejaEntrada.Where(a => a.ClaveReceptor == Clave).FirstOrDefault();
                            if (Documentos != null)
                            {
                                db.Entry(Documentos).State = System.Data.Entity.EntityState.Modified;
                                Documentos.RespuestaHacienda = resp2.hacienda_result.ind_estado;
                                Documentos.ConsecutivoReceptor = Documentos.ClaveReceptor.Substring(21, 20);
                                db.SaveChanges();
                            }
                        }
                        catch (Exception ex)
                        {

                           
                        }
                       


                        G.CerrarConexionAPP(db);

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        G.CerrarConexionAPP(db);

                        return Request.CreateResponse(HttpStatusCode.OK, response2.IsSuccessStatusCode);
                    }
                }
                catch (Exception ex)
                {
                    G.CerrarConexionAPP(db);

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

                }

                //

            }
            catch (Exception ex)
            {
                G.CerrarConexionAPP(db);

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }



        [Route("api/Documentos/Respuesta")]
        [HttpPost]
        public async System.Threading.Tasks.Task<HttpResponseMessage> PostAsync([FromBody] data datos)
        {

            try
            {
                G.AbrirConexionAPP(out db);

                var item = db.EncDocumento.Where(a => a.ClaveHacienda == datos.clave).FirstOrDefault();
                var parametros = db.Parametros.FirstOrDefault();
                if (item != null)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    item.RespuestaHacienda = datos.ind_estado;

                    

                    db.SaveChanges();
                }
                else
                {
                    var ClaveRespuesta = datos.clave.Split('-');
                    string Clave = ClaveRespuesta[0];
                    var bandeja = db.BandejaEntrada.Where(a => a.ClaveReceptor == Clave).FirstOrDefault();

                    if (bandeja != null)
                    {
                        db.Entry(bandeja).State = System.Data.Entity.EntityState.Modified;
                        bandeja.XMLRespuesta = datos.respuesta_xml;
                        bandeja.RespuestaHacienda = datos.ind_estado;
                        bandeja.ConsecutivoReceptor = ClaveRespuesta[1];
                        db.SaveChanges();
                    }
                    else
                    {
                        BitacoraErrores be = new BitacoraErrores();
                        be.DocNum = "";
                        be.Type = "";
                        be.Descripcion = "No se encontro " + JsonConvert.SerializeObject(datos);
                        be.StackTrace = "Respuesta";
                        be.Fecha = DateTime.Now;
                        db.BitacoraErrores.Add(be);
                        db.SaveChanges();
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