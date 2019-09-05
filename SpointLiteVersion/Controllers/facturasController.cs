using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpointLiteVersion.Models;
using Rotativa;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace SpointLiteVersion.Controllers
{
    public class facturasController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: facturas
        public ActionResult Index()
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            return View(db.facturas.Where(m=>m.Status=="PAGADA" && m.empresaid==empresaid1 || m.Status=="PENDIENTE" && m.empresaid==empresaid1).ToList());
        }
        public ActionResult CotizacionesIndex()
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);

            return View(db.cotizacion.Where(m=>m.empresaid==empresaid1 && m.Status=="PAGADA" || m.empresaid==empresaid1 && m.Status=="PENDIENTE").ToList());
        }

        public ActionResult PrefacturaIndex()
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);

            return View(db.prefactura.Where(m=>m.Status=="PAGADA" && m.empresaid==empresaid1 || m.Status=="PREFACTURA" && m.empresaid == empresaid1).ToList());
        }
        public ActionResult VistaPdf(string idProducto)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            ViewBag.cantidad = idProducto;
            return View();
        }
        public JsonResult GetDatosProductos(string search)
        {
         
            //Searching records from list using LINQ query  
            var CityList = (from N in db.productos
                            where N.Descripcion.StartsWith(search)
                            select new { N.Descripcion });
            return Json(CityList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PDf(string fecha, string creditos, string observacion,string descripcionncf,string idncfenviar,string ncf,string itbisdado, string subtotaldado, string cliente, string idcliente, string vendedor, string precio, string credito, string Total, List<DetalleVenta> ListadoDetalle)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            string mensaje = "";
            var cantidadsuficiente = false;
            string creditos1 = creditos;
            Decimal creditosdisponibles = 0;
            foreach (var data in ListadoDetalle)
            {
                string idProducto = data.Ref.ToString();
                int cantidad = Convert.ToInt32(data.cantidad.ToString());
                if (idProducto != "")
                {
                    var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                    if (q.cantidad == 0)
                    {
                        mensaje = "Error Producto Agotado";
                        cantidadsuficiente = false;
                    }else if (q.cantidad < cantidad)
                    {
                        mensaje = "La Cantidad Supera Al Stock tienes actualmente "+q.cantidad+" productos de este tipo en tu inventario";
                        cantidadsuficiente = false;

                    }
                    else
                    {
                        cantidadsuficiente = true;
                    }
                }
            }
            if (cantidadsuficiente == true)
            {
                if (fecha == ""  || cliente == "" || vendedor == "")
                {
                    if (fecha == "") mensaje = "ERROR EN EL CAMPO FECHA";
                    if (cliente == "") mensaje = "ERROR CON EL CLIENTE";
                    if (vendedor == "") mensaje = "ERROR EN EL CAMPO VENDEDOR";


                }
                else if (credito == "si")
                {
                    var idclien = Convert.ToInt32(idcliente);
                    System.Console.WriteLine("" + idclien);
                    var dato1 = (from datos in db.facturas where datos.idcliente == idclien && datos.Status == "PENDIENTE" select datos).FirstOrDefault();
                    if (dato1 != null)
                    {

                        creditosdisponibles = Convert.ToDecimal(creditos1) - Convert.ToDecimal(dato1.Total);
                        if (creditosdisponibles > 0 && creditosdisponibles < Convert.ToDecimal(Total) || creditosdisponibles < 0)
                        {
                            mensaje = "La Factura Excede El límite de Crédito del Cliente el cual cuenta actualmente con " + "$" + creditosdisponibles.ToString("00");

                        }
                    }



                    else if (Convert.ToDecimal(creditos1) > 0 && Convert.ToDecimal(creditos1) < Convert.ToDecimal(Total))
                    {
                        mensaje = "La Factura Excede El límite de Crédito del Cliente el cual cuenta con " + "$" + creditos + " Aprobado";
                    }
                    else if (Convert.ToDecimal(creditos1) > 0 || Convert.ToDecimal(creditos1) >= Convert.ToDecimal(Total))
                    {
                        if (precio == "") precio = "0.0";
                        if (Total == "") Total = "0.00";
                        int id1 = 0;
                        DetalleVenta detalle = new DetalleVenta();
                        var verificar = (from s in db.DetalleVenta select s.IdDetalle);
                        if (verificar.Count() > 0)
                        {
                            id1 = (from s in db.DetalleVenta select s.IdDetalle).Max();
                        }
                        int idventa = id1 + 1;
                        facturas factura = new facturas();
                        factura.cliente = cliente;
                        factura.fecha = Convert.ToDateTime(fecha);
                        factura.observacion = observacion;
                        factura.vendedor = vendedor;
                        factura.precio = Convert.ToDecimal(precio);
                        factura.credito = credito;
                        factura.idventa = idventa;
                        factura.Total = Convert.ToDecimal(Total);
                        factura.Status = "PENDIENTE";
                        factura.idcliente = Convert.ToInt32(idcliente);
                        factura.empresaid = empresaid1;
                        factura.usuarioid = usuarioid1;
                        factura.totalitbis = Convert.ToDecimal(itbisdado);
                        factura.subtotal = Convert.ToDecimal(subtotaldado);

                        factura.Descripcionncf = descripcionncf;
                        db.facturas.Add(factura);
                        db.SaveChanges();
                        int id = factura.idfactura;
                        productos producto = new productos();


                        foreach (var data in ListadoDetalle)
                        {
                            string idProducto = data.Ref.ToString();
                            int cantidad = Convert.ToInt32(data.cantidad.ToString());
                            if (idProducto != "")
                            {
                                var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                                q.cantidad = q.cantidad - cantidad;
                                var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).FirstOrDefault();
                                qs.cantidad = qs.cantidad - cantidad;
                                qs.Tipo = "VENTA";
                                qs.Fecha = Convert.ToDateTime(fecha);
                                db.SaveChanges();
                            }
                            decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                            decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                            decimal total = Convert.ToDecimal(data.total.ToString());
                            decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                            string descripcion1 = data.descripcion.ToString();
                            decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                            string itbis = data.itbis.ToString();
                            DetalleVenta objDetalleVenta = new DetalleVenta(id, idventa, idProducto, descripcion1, cantidad, precio1, descuento, itbis, subtotal, total, totaldescuento, empresaid1, usuarioid1, 1);
                            DetalleCompra objDetalle = new DetalleCompra(idProducto, id, cantidad, descripcion1, precio1, total, Convert.ToDateTime(fecha), "VENTA", empresaid1, usuarioid1, 1);

                            Session["idVenta"] = idventa;
                            db.DetalleCompra.Add(objDetalle);
                            db.DetalleVenta.Add(objDetalleVenta);
                            db.SaveChanges();
                            if (idncfenviar != "" || idncfenviar != null)
                            {
                                DetalleNcf secuenciancf = new DetalleNcf();
                                secuenciancf.idNcf = Convert.ToInt32(idncfenviar);
                                secuenciancf.iddocumento = id;
                                secuenciancf.tipodocumento = 1;
                                var sec = (from i in db.DetalleNcf select i.secuencia).Max();
                                if (sec >= 0)
                                {
                                    secuenciancf.secuencia = sec + 1;
                                }
                                else if (sec < 0)
                                {
                                    secuenciancf.secuencia = 1;
                                }
                                secuenciancf.status = 1;
                                secuenciancf.empresaid = empresaid1;
                                secuenciancf.usuarioid = usuarioid1;
                                var idncfcomp = Convert.ToInt32(idncfenviar);
                                var idncfsecuencia = (from i in db.NcFsecuencia where i.idncf == idncfcomp select i.idDetalle).FirstOrDefault();
                                if (idncfsecuencia >= 0)
                                {
                                    secuenciancf.idncfsecuencia = idncfsecuencia;
                                }
                                db.SaveChanges();

                                db.NCFCALCULO(secuenciancf.secuencia, idventa, ncf);
                            }
                        }
                        mensaje = "VENTA GUARDADA CON EXITO...";

                    }
                }


                else
                {
                    if (precio == "") precio = "0.0";
                    if (Total == "") Total = "0.00";
                    int id1 = 0;
                    DetalleVenta detalle = new DetalleVenta();
                    var verificar = (from s in db.DetalleVenta select s.IdDetalle);
                    if (verificar.Count() > 0)
                    {
                        id1 = (from s in db.DetalleVenta select s.IdDetalle).Max();
                    }
                    int idventa = id1 + 1;
                    facturas factura = new facturas();
                    factura.cliente = cliente;
                    factura.fecha = Convert.ToDateTime(fecha);
                    factura.observacion = observacion;
                    factura.vendedor = vendedor;
                    factura.precio = Convert.ToDecimal(precio);
                    factura.credito = credito;
                    factura.idventa = idventa;
                    factura.Total = Convert.ToDecimal(Total);
                    factura.Status = "PAGADA";
                    factura.idcliente = Convert.ToInt32(idcliente);
                    factura.empresaid = empresaid1;
                    factura.totalitbis = Convert.ToDecimal(itbisdado);
                    factura.subtotal = Convert.ToDecimal(subtotaldado);
                    factura.Descripcionncf = descripcionncf;
                    factura.usuarioid = usuarioid1;
                    db.facturas.Add(factura);
                    db.SaveChanges();
                    int id = factura.idfactura;


                    foreach (var data in ListadoDetalle)
                    {
                        string idProducto = data.Ref.ToString();
                        int cantidad = Convert.ToInt32(data.cantidad.ToString());
                        if (idProducto != "")
                        {
                            var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                            q.cantidad = q.cantidad - cantidad;
                            var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).FirstOrDefault();
                            qs.cantidad = qs.cantidad - cantidad;
                            qs.Tipo = "VENTA";
                            qs.Fecha = Convert.ToDateTime(fecha);
                            db.SaveChanges();
                        }
                        decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                        decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                        decimal total = Convert.ToDecimal(data.total.ToString());
                        decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                        string descripcion1 = data.descripcion.ToString();
                        decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                        string itbis = data.itbis.ToString();
                        DetalleVenta objDetalleVenta = new DetalleVenta(id, idventa, idProducto, descripcion1, cantidad, precio1, descuento, itbis, subtotal, total, totaldescuento, empresaid1, usuarioid1,1);
                        Session["idVenta"] = idventa;
                        DetalleCompra objDetalle = new DetalleCompra(idProducto, id, cantidad, descripcion1, precio1, total, Convert.ToDateTime(fecha), "VENTA", empresaid1, usuarioid1,1);

                        db.DetalleCompra.Add(objDetalle);
                        db.SaveChanges();
                        db.DetalleVenta.Add(objDetalleVenta);
                        db.SaveChanges();

                        if (idncfenviar != "" || idncfenviar != null)
                        {
                            DetalleNcf secuenciancf = new DetalleNcf();
                            secuenciancf.idNcf = Convert.ToInt32(idncfenviar);
                            secuenciancf.iddocumento = id;
                            secuenciancf.tipodocumento = 1;
                            var sec = (from i in db.DetalleNcf select i.secuencia).Max();
                            if (sec >= 0)
                            {
                                secuenciancf.secuencia = sec + 1;
                            }
                            else if (sec < 0 || sec==null )
                            {
                                secuenciancf.secuencia = 1;
                            }
                            secuenciancf.status = 1;
                            secuenciancf.empresaid = empresaid1;
                            secuenciancf.usuarioid = usuarioid1;
                            var idncfcomp = Convert.ToInt32(idncfenviar);
                            var idncfsecuencia = (from i in db.NcFsecuencia where i.idncf == idncfcomp select i.idDetalle).FirstOrDefault();
                            if (idncfsecuencia >= 0)
                            {
                                secuenciancf.idncfsecuencia = idncfsecuencia;
                            }
                            db.DetalleNcf.Add(secuenciancf);
                            db.SaveChanges();
                            int secuenciavalidada = Convert.ToInt32(secuenciancf.secuencia);
                            db.NCFCALCULO(secuenciavalidada, idventa, ncf);
                        }
                    }
                    mensaje = "VENTA GUARDADA CON EXITO...";


                }
            }
            return Json(mensaje);



        }

        //metodo para realizar prefacturas
        public ActionResult prefactura(string fecha, string creditos, string observacion, string itbisdado,string subtotaldado,string cliente, string idcliente, string vendedor, string precio, string credito, string Total, List<DetallePrefactura> ListadoDetalle)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            string mensaje = "";
            string creditos1 = creditos;
            Decimal creditosdisponibles = 0;
            if (fecha == "" || observacion == "" || cliente == "" || vendedor == "")
            {
                if (fecha == "") mensaje = "ERROR EN EL CAMPO FECHA";
                if (observacion == "") mensaje = "ERROR EN EL CAMPO OBSERVACIÓN";
                if (cliente == "") mensaje = "ERROR CON EL CLIENTE";
                if (vendedor == "") mensaje = "ERROR EN EL CAMPO VENDEDOR";


            }
            else if (credito == "si")
            {
                var idclien = Convert.ToInt32(idcliente);
                System.Console.WriteLine("" + idclien);
                var dato1 = (from datos in db.prefactura where datos.idcliente == idclien && datos.Status == "PENDIENTE" select datos).FirstOrDefault();
                if (dato1 != null)
                {

                    creditosdisponibles = Convert.ToDecimal(creditos1) - Convert.ToDecimal(dato1.Total);
                    if (creditosdisponibles > 0 && creditosdisponibles < Convert.ToDecimal(Total) || creditosdisponibles < 0)
                    {
                        mensaje = "La Factura Excede El límite de Crédito del Cliente el cual cuenta actualmente con " + "$" + creditosdisponibles.ToString("00");

                    }
                }



                else if (Convert.ToDecimal(creditos1) > 0 && Convert.ToDecimal(creditos1) < Convert.ToDecimal(Total))
                {
                    mensaje = "La Factura Excede El límite de Crédito del Cliente el cual cuenta con " + "$" + creditos + " Aprobado";
                }
                else if (Convert.ToDecimal(creditos1) > 0 || Convert.ToDecimal(creditos1) >= Convert.ToDecimal(Total))
                {
                    if (precio == "") precio = "0.0";
                    if (Total == "") Total = "0.00";
                    int id1 = 0;
                    int id2 = 0;
                    DetallePrefactura detalle = new DetallePrefactura();
                    var verificar = (from s in db.DetallePrefactura select s.IdDetalle);
                    if (verificar.Count() > 0)
                    {
                        id1 = (from s in db.DetallePrefactura select s.IdDetalle).Max();
                    }
                    var verificar1 = (from s in db.facturas select s.idfactura);
                    if (verificar1.Count() > 0)
                    {
                        id2 = Convert.ToInt32((from s in db.facturas select s.idfactura).Max());
                    }
                    int idventa = id1 + 1;
                    int idfacturaagregar = id2 + 1;

                    prefactura factura = new prefactura();
                    factura.cliente = cliente;
                    factura.fecha = Convert.ToDateTime(fecha);
                    factura.observacion = observacion;
                    factura.vendedor = vendedor;
                    factura.precio = Convert.ToDecimal(precio);
                    factura.credito = credito;
                    factura.idventa = idventa;
                    factura.Total = Convert.ToDecimal(Total);
                    factura.Status = "PREFACTURA";
                    factura.empresaid = empresaid1;
                    factura.usuarioid = usuarioid1;
                    factura.idcliente = Convert.ToInt32(idcliente);
                    factura.totalitbis = Convert.ToDecimal(itbisdado);
                    factura.subtotal = Convert.ToDecimal(subtotaldado);

                    db.prefactura.Add(factura);
                    db.SaveChanges();
                    int id = factura.idprefactura;
                    productos producto = new productos();


                    foreach (var data in ListadoDetalle)
                    {
                        string idProducto = data.Ref.ToString();
                        int cantidad = Convert.ToInt32(data.cantidad.ToString());
                    
                        decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                        decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                        decimal total = Convert.ToDecimal(data.total.ToString());
                        decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                        string descripcion1 = data.descripcion.ToString();
                        decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                        string itbis = data.itbis.ToString();
                        DetallePrefactura objDetalleVenta = new DetallePrefactura(id, idventa, idProducto, descripcion1, cantidad, precio1, descuento, itbis, subtotal, total, totaldescuento,idfacturaagregar,empresaid1,usuarioid1,1);

                        Session["idVenta"] = idventa;
                        db.DetallePrefactura.Add(objDetalleVenta);
                        db.SaveChanges();


                    }
                    mensaje = "PREFACTURA GUARDADA CON EXITO...";

                }
            }


            else
            {
                if (precio == "") precio = "0.0";
                if (Total == "") Total = "0.00";
                var id1 = 0;
                var id2 = 0;
                DetallePrefactura detalle = new DetallePrefactura();
                var verificar = (from s in db.DetallePrefactura select s.IdDetalle);
                if (verificar.Count() > 0)
                {
                    id1 =Convert.ToInt32((from s in db.DetallePrefactura select s.IdDetalle).Max());
                }
                var verificar1 = (from s in db.facturas select s.idfactura);
                if (verificar1.Count() > 0)
                {
                    id2 = Convert.ToInt32((from s in db.facturas select s.idfactura).Max());
                }
                int idventa = id1 + 1;
                int idfacturaagregar = id2 + 1;
                prefactura factura = new prefactura();
                factura.cliente = cliente;
                factura.fecha = Convert.ToDateTime(fecha);
                factura.observacion = observacion;
                factura.vendedor = vendedor;
                factura.precio = Convert.ToDecimal(precio);
                factura.credito = credito;
                factura.idventa = idventa;
                factura.totalitbis = Convert.ToDecimal(itbisdado);
                factura.Total = Convert.ToDecimal(Total);
                factura.Status = "PREFACTURA";
                factura.idcliente = Convert.ToInt32(idcliente);
                factura.empresaid = empresaid1;
                factura.usuarioid = usuarioid1;
                factura.subtotal = Convert.ToDecimal(subtotaldado);

                db.prefactura.Add(factura);
                db.SaveChanges();
                int id = factura.idprefactura;


                foreach (var data in ListadoDetalle)
                {
                    string idProducto = data.Ref.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                  
                    decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                    decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                    decimal total = Convert.ToDecimal(data.total.ToString());
                    decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                    string descripcion1 = data.descripcion.ToString();
                    decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                    string itbis = data.itbis.ToString();
                    DetallePrefactura objDetalleVenta = new DetallePrefactura(id, idventa, idProducto, descripcion1, cantidad, precio1, descuento, itbis, subtotal, total, totaldescuento, idfacturaagregar,empresaid1,usuarioid1,1);
                    Session["idVenta"] = idventa;

                    db.DetallePrefactura.Add(objDetalleVenta);
                    db.SaveChanges();


                }
                mensaje = "PREFACTURA GUARDADA CON EXITO...";


            }
            return Json(mensaje);



        }
        //fin de las prefacturas
        //metodo para realizar las cotizaciones
       
        public ActionResult Cotizar(string fecha, string creditos, string observacion, string itbisdado,string subtotaldado,string cliente, string idcliente, string vendedor, string precio, string credito, string Total, List<DetalleCotizacion> ListadoDetalle)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            string mensaje = "";
            string creditos1 = creditos;
            Decimal creditosdisponibles = 0;
            if (fecha == "" || observacion == "" || cliente == "" || vendedor == "")
            {
                if (fecha == "") mensaje = "ERROR EN EL CAMPO FECHA";
                if (observacion == "") mensaje = "ERROR EN EL CAMPO OBSERVACIÓN";
                if (cliente == "") mensaje = "ERROR CON EL CLIENTE";
                if (vendedor == "") mensaje = "ERROR EN EL CAMPO VENDEDOR";


            }
            else if (credito == "si")
            {
                var idclien = Convert.ToInt32(idcliente);
                System.Console.WriteLine("" + idclien);
                var dato1 = (from datos in db.facturas where datos.idcliente == idclien && datos.Status == "PENDIENTE" select datos).FirstOrDefault();
                if (dato1 != null)
                {

                    creditosdisponibles = Convert.ToDecimal(creditos1) - Convert.ToDecimal(dato1.Total);
                    if (creditosdisponibles > 0 && creditosdisponibles < Convert.ToDecimal(Total) || creditosdisponibles < 0)
                    {
                        mensaje = "La Factura Excede El límite de Crédito del Cliente el cual cuenta actualmente con " + "$" + creditosdisponibles.ToString("00");

                    }
                }



                else if (Convert.ToDecimal(creditos1) > 0 && Convert.ToDecimal(creditos1) < Convert.ToDecimal(Total))
                {
                    mensaje = "La Factura Excede El límite de Crédito del Cliente el cual cuenta con " + "$" + creditos + " Aprobado";
                }
                else if (Convert.ToDecimal(creditos1) > 0 && Convert.ToDecimal(creditos1) >= Convert.ToDecimal(Total))
                {
                    if (precio == "") precio = "0.0";
                    if (Total == "") Total = "0.00";
                    int id1 = 0;
                    DetalleCotizacion detalle = new DetalleCotizacion();
                    var verificar = (from s in db.DetalleCotizacion select s.idDetalle);
                    if (verificar.Count() > 0)
                    {
                        id1 = (from s in db.DetalleCotizacion select s.idDetalle).Max();
                    }
                    int idventa = id1 + 1;
                    cotizacion factura = new cotizacion();
                    factura.cliente = cliente;
                    factura.fecha = Convert.ToDateTime(fecha);
                    factura.observacion = observacion;
                    factura.vendedor = vendedor;
                    factura.precio = Convert.ToDecimal(precio);
                    factura.credito = credito;
                    factura.idventa = idventa;
                    factura.Total = Convert.ToDecimal(Total);
                    factura.Status = "PENDIENTE";
                    factura.idcliente = Convert.ToInt32(idcliente);
                    factura.totalitbis = Convert.ToDecimal(itbisdado);
                    factura.empresaid = empresaid1;
                    factura.usuarioid = usuarioid1;
                    factura.subtotal = Convert.ToDecimal(subtotaldado);

                    db.cotizacion.Add(factura);
                    db.SaveChanges();
                    int id = factura.idcotizacion;
                    productos producto = new productos();


                    foreach (var data in ListadoDetalle)
                    {
                        string idProducto = data.Ref.ToString();
                        int cantidad = Convert.ToInt32(data.cantidad.ToString());
                  
                        decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                        decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                        decimal total = Convert.ToDecimal(data.total.ToString());
                        decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                        string descripcion1 = data.descripcion.ToString();
                        decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                        string itbis = data.itbis.ToString();
                        DetalleCotizacion objDetalleVenta = new DetalleCotizacion(id, idProducto, descripcion1, cantidad, precio1, descuento, itbis, subtotal, total, totaldescuento,idventa,empresaid1,usuarioid1,1);

                        Session["idVenta"] = idventa;
                        db.DetalleCotizacion.Add(objDetalleVenta);
                        db.SaveChanges();


                    }
                    mensaje = "VENTA COTIZADA CON EXITO...";

                }
            }


            else
            {
                if (precio == "") precio = "0.0";
                if (Total == "") Total = "0.00";
                int id1 = 0;
                DetalleCotizacion detalle = new DetalleCotizacion();
                var verificar = (from s in db.DetalleCotizacion select s.idDetalle);
                if (verificar.Count() > 0)
                {
                    id1 = (from s in db.DetalleCotizacion select s.idDetalle).Max();
                }
                int idventa = id1 + 1;
                cotizacion factura = new cotizacion();
                factura.cliente = cliente;
                factura.fecha = Convert.ToDateTime(fecha);
                factura.observacion = observacion;
                factura.vendedor = vendedor;
                factura.precio = Convert.ToDecimal(precio);
                factura.credito = credito;
                factura.idventa = idventa;
                factura.Total = Convert.ToDecimal(Total);
                factura.Status = "PAGADA";
                factura.idcliente = Convert.ToInt32(idcliente);
                factura.empresaid = empresaid1;
                factura.usuarioid = usuarioid1;
                factura.totalitbis = Convert.ToDecimal(itbisdado);
                factura.subtotal = Convert.ToDecimal(subtotaldado);

                db.cotizacion.Add(factura);
                db.SaveChanges();
                int id = factura.idcotizacion;


                foreach (var data in ListadoDetalle)
                {
                    string idProducto = data.Ref.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                   
                    decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                    decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                    decimal total = Convert.ToDecimal(data.total.ToString());
                    decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                    string descripcion1 = data.descripcion.ToString();
                    decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                    string itbis = data.itbis.ToString();
                    DetalleCotizacion objDetalleVenta = new DetalleCotizacion(id, idProducto, descripcion1, cantidad, precio1, descuento, itbis, subtotal, total, totaldescuento,idventa,empresaid1,usuarioid1,1);
                    Session["idVenta"] = idventa;

                    db.DetalleCotizacion.Add(objDetalleVenta);
                    db.SaveChanges();


                }
                
                mensaje = "COTIZACION REALIZADA CON EXITO...";



            }
            return Json(mensaje);



        }
        public ActionResult Detailscotizacion(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/cotizar.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }

            return View(facturas);
        }
        // GET: facturas/Details/5
        public ActionResult Details(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/WebForm1.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
          
            return View(facturas);
        }

        public ActionResult DetailsPrefactura(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/prefactura.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }

            return View(facturas);
        }

        public ActionResult CobrarPrefactura(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
         

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                List<DetallePrefactura> detalle = (from s in db.DetallePrefactura where s.idventa==id select s).ToList();
                foreach (var data in detalle)
                {
                    string idProducto = data.Ref.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                    if (idProducto != "")
                    {
                        var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                        q.cantidad = q.cantidad - cantidad;
                        var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).FirstOrDefault();
                        qs.cantidad = qs.cantidad - cantidad;
                        qs.Tipo = "VENTA";
                        var qt = (from a in db.prefactura where a.idventa == id select a).FirstOrDefault();
                        qt.Status = "PAGADA";
                        db.SaveChanges();
                    }
                    var fecha1 = (from n in db.prefactura where n.idventa == id select n).FirstOrDefault();
                    var fecha = Convert.ToString(fecha1.fecha.ToString());
                    decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                    decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                    decimal total = Convert.ToDecimal(data.total.ToString());
                    decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                    string descripcion1 = data.descripcion.ToString();
                    decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                    string itbis = data.itbis.ToString();
                    DetalleCompra objDetalle = new DetalleCompra(idProducto, id, cantidad, descripcion1, precio1, total, Convert.ToDateTime(fecha), "VENTA", empresaid1, usuarioid1,1);
                    db.DetalleCompra.Add(objDetalle);
                    db.SaveChanges();
               
                }
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/prefactura.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }

            return View(facturas);
        }

        public ActionResult Cobrarfactura(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
          

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                List<DetalleVenta> detalle = (from s in db.DetalleVenta where s.idventa == id select s).ToList();
                foreach (var data in detalle)
                {
                    string idProducto = data.Ref.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                    if (idProducto != "")
                    {
                        var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                        q.cantidad = q.cantidad - cantidad;
                        var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).FirstOrDefault();
                        qs.cantidad = qs.cantidad - cantidad;
                        qs.Tipo = "VENTA";
                        var qt = (from a in db.facturas where a.idventa == id select a).FirstOrDefault();
                        qt.Status = "PAGADA";
                        db.SaveChanges();
                    }
                    var fecha1 = (from n in db.prefactura where n.idventa == id select n).FirstOrDefault();
                    var fecha = Convert.ToString(fecha1.fecha.ToString());
                    decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                    decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                    decimal total = Convert.ToDecimal(data.total.ToString());
                    decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                    string descripcion1 = data.descripcion.ToString();
                    decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                    string itbis = data.itbis.ToString();
                    DetalleCompra objDetalle = new DetalleCompra(idProducto, id, cantidad, descripcion1, precio1, total, Convert.ToDateTime(fecha), "VENTA", empresaid1, usuarioid1,1);
                    db.DetalleCompra.Add(objDetalle);
                    db.SaveChanges();

                }
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/WebForm1.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }

            return View(facturas);
        }

        //cobrar cotizacion

        public ActionResult CobrarCotizacion(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                List<DetalleCotizacion> detalle = (from s in db.DetalleCotizacion where s.idventa == id select s).ToList();
                foreach (var data in detalle)
                {
                    string idProducto = data.Ref.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                    if (idProducto != "")
                    {
                        var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                        q.cantidad = q.cantidad - cantidad;
                        var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).FirstOrDefault();
                        qs.cantidad = qs.cantidad - cantidad;
                        qs.Tipo = "VENTA";
                        var qt = (from a in db.cotizacion where a.idventa == id select a).FirstOrDefault();
                        qt.Status = "PAGADA";
                        db.SaveChanges();
                    }
                    var fecha1 = (from n in db.cotizacion where n.idventa == id select n).FirstOrDefault();
                    var fecha = Convert.ToString(fecha1.fecha.ToString());
                    decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                    decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                    decimal total = Convert.ToDecimal(data.total.ToString());
                    decimal totaldescuento = Convert.ToDecimal(data.totaldescuento.ToString());
                    string descripcion1 = data.descripcion.ToString();
                    decimal precio1 = Convert.ToDecimal(data.precio.ToString());
                    string itbis = data.itbis.ToString();
                    DetalleCompra objDetalle = new DetalleCompra(idProducto, id, cantidad, descripcion1, precio1, total, Convert.ToDateTime(fecha), "VENTA", empresaid1, usuarioid1, 1);
                    db.DetalleCompra.Add(objDetalle);
                    db.SaveChanges();

                }
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/cotizar.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }

            return View(facturas);
        }
        public ActionResult Anularfactura(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                List<DetalleVenta> detalle = (from s in db.DetalleVenta where s.idventa == id select s).ToList();
                List<DetalleCompra> detalle1 = (from s in db.DetalleCompra where s.idcompra == id select s).ToList();
                foreach(var dat in detalle1)
                {
                    string idProducto = dat.codproducto.ToString();
                    if (idProducto != "")
                    {
                        dat.estatus = 0;
                        db.SaveChanges();
                    }
                    }
                foreach (var data in detalle)
                {
                    string idProducto = data.Ref.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                    if (idProducto != "")
                    {
                        var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                        q.cantidad = q.cantidad + cantidad;
                        var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).FirstOrDefault();
                        qs.cantidad = qs.cantidad + cantidad;
                        var qt = (from a in db.facturas where a.idventa == id select a).FirstOrDefault();
                        qt.Status = "ANULADA";
                        data.estatus = 0;
                        db.SaveChanges();
                    }
           

                }
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/WebForm1.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }

            return View(facturas);
        }


        //anular cotizacion

        public ActionResult Anularcotizacion(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
           

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                var cotiza = (from l in db.cotizacion where l.idventa == id select l).FirstOrDefault();
                List<DetalleCotizacion> detalle = (from s in db.DetalleCotizacion where s.idventa == id select s).ToList();
                List<DetalleCompra> detalle1 = (from s in db.DetalleCompra where s.idcompra == id select s).ToList();
                foreach (var dat in detalle1)
                {
                    string idProducto = dat.codproducto.ToString();
                    if (idProducto != "")
                    {
                        cotiza.Status ="0";
                        dat.estatus = 0;
                        db.SaveChanges();
                    }
                }
                foreach (var data in detalle1)
                {
                    string idProducto = data.codproducto.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                    if (idProducto != "")
                    {
                        var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                        q.cantidad = q.cantidad + cantidad;
                        var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).FirstOrDefault();
                        qs.cantidad = qs.cantidad + cantidad;
                        var qt = (from a in db.facturas where a.idventa == id select a).FirstOrDefault();
                        qt.Status = "ANULADA";
                        data.estatus = 0;
                        db.SaveChanges();
                    }


                }
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/cotizar.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }

            return View(facturas);
        }

        //WLIMINarFACTIRA
        public ActionResult Anularprefactua(int? id)
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (id != null)
            {
                var cotiza = (from l in db.prefactura where l.idventa == id select l).FirstOrDefault();
                List<DetallePrefactura> detalle = (from s in db.DetallePrefactura where s.idventa == id select s).ToList();
                List<DetalleCompra> detalle1 = (from s in db.DetalleCompra where s.idcompra == id select s).ToList();
                foreach (var dat in detalle1)
                {
                    string idProducto = dat.codproducto.ToString();
                    if (idProducto != "")
                    {
                        cotiza.Status = "0";
                        dat.estatus = 0;
                        db.SaveChanges();
                    }
                }
                foreach (var data in detalle1)
                {
                    string idProducto = data.codproducto.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                    if (idProducto != "")
                    {
                        var q = (from a in db.productos where a.CodProducto == idProducto select a).FirstOrDefault();
                        q.cantidad = q.cantidad + cantidad;
                        var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).FirstOrDefault();
                        qs.cantidad = qs.cantidad + cantidad;
                        var qt = (from a in db.facturas where a.idventa == id select a).FirstOrDefault();
                        qt.Status = "ANULADA";
                        data.estatus = 0;
                        db.SaveChanges();
                    }


                }
                string idVenta = id.ToString();
                return Redirect("~/RTPFactura/prefactura.aspx?IdVenta=" + idVenta);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }

            return View(facturas);
        }
        public ActionResult reporteActual()
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (Session["idVenta"].ToString() != null)
            {
                string idVenta = Session["idVenta"].ToString();
                return Redirect("~/RTPFactura/WebForm1.aspx?idventa="+ idVenta);
            }
            else
            {
                return View("PDf");
            }
            //return Redirect("~/RTPFactura/WebForm1.aspx?idventa="+001);
           
         
                
            

        }
        public ActionResult cotizacionActual()
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (Session["idVenta"].ToString() != null)
            {
                string idVenta = Session["idVenta"].ToString();
                return Redirect("~/RTPFactura/cotizar.aspx?idventa=" + idVenta);
            }
            else
            {
                return View("PDf");
            }
            //return Redirect("~/RTPFactura/WebForm1.aspx?idventa="+001);





        }
        // GET: facturas/Create
        public ActionResult Create()
        {
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);

            

            ViewBag.cliente = new SelectList(db.clientes.Where(m => m.Status == "1" && m.empresaid==empresaid1), "idcliente", "nombre");
            ViewBag.vendedor = new SelectList(db.vendedores.Where(m => m.Status == "1" && m.empresaid==empresaid1), "idvendedor", "nombre");
            ViewBag.ncf = new SelectList(db.NCF.Where(m => m.Estatus == "1" && m.empresaid == empresaid1), "idNCF", "NombreComp");

            var ListadoDetalle = new List<DetalleVenta>();
            ListadoDetalle.Add(new DetalleVenta());
            ListadoDetalle.Add(new DetalleVenta());
           
            ViewBag.producto = new SelectList(db.productos.Where(m => m.Status == "1" && m.empresaid==empresaid1), "idProducto", "Descripcion");
            return View();
        }

        public JsonResult GetNcf(int idproducto)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            db.Configuration.ProxyCreationEnabled = false;
            List<NCF> productos = db.NCF.Where(m => m.idNCF == idproducto && m.Estatus == "1" && m.empresaid == empresaid1).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(productos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getproducto(int idproducto)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            db.Configuration.ProxyCreationEnabled = false;
            List<productos> productos = db.productos.Where(m => m.idProducto == idproducto && m.Status=="1" && m.empresaid==empresaid1).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(productos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getcliente(int idproducto)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<clientes> productos = db.clientes.Where(m => m.idCliente == idproducto && m.Status == "1").ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(productos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Getvendedor(int idproducto)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<vendedores> productos = db.vendedores.Where(m => m.idvendedor == idproducto && m.Status == "1").ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(productos, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult Getproducto(int idproducto)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    List<productos> productos = db.productos.Where(m => m.idProducto == idproducto).ToList();

        //    //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
        //    return Json(productos, JsonRequestBehavior.AllowGet);
        //}

        // POST: facturas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idfactura,fecha,observacion,cliente,vendedor,producto,descripcion,precio,credito")]facturas facturas, List<DetalleVenta> ListadoDetalle)
        {
            if (ModelState.IsValid)
            {
                db.facturas.Add(facturas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            string mensaje = "";

            facturas factura = new facturas();

           
            mensaje = "VENTA GUARDADA CON EXITO...";

            return Json(mensaje);
            //return View(facturas);
        }

        // GET: facturas/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            return View(facturas);
        }

        // POST: facturas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idfactura,fecha,observacion,cliente,vendedor,producto,descripcion,precio,credito")] facturas facturas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facturas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facturas);
        }

        // GET: facturas/Delete/5
        public ActionResult Delete(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            facturas facturas = db.facturas.Find(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            return View(facturas);
        }

        // POST: facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            facturas facturas = db.facturas.Find(id);
            db.facturas.Remove(facturas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
