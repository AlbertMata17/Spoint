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
            return View(db.facturas.ToList());
        }
        public ActionResult VistaPdf(string idProducto)
        {
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
        public ActionResult PDf(string fecha,string creditos,string observacion, string cliente,string idcliente, string vendedor, string precio, string credito,string Total,List<DetalleVenta> ListadoDetalle)
        {
            string mensaje = "";
            string creditos1 = creditos;
            Decimal creditosdisponibles=0;
            if (fecha == "" || observacion == "" || cliente == "" || vendedor == "" || credito == "")
            {
                if (fecha == "") mensaje = "ERROR EN EL CAMPO FECHA";
                if (observacion == "") mensaje = "ERROR EN EL CAMPO OBSERVACIÓN";
                if (cliente == "") mensaje = "ERROR CON EL CLIENTE";
                if (vendedor == "") mensaje = "ERROR EN EL CAMPO VENDEDOR";
                if (credito == "") mensaje = "ERROR EN EL CAMPO CREDITO";


            } else if (credito == "si") {
                var idclien = Convert.ToInt32(idcliente);
                System.Console.WriteLine(""+ idclien);
                var dato1 = (from datos in db.facturas where datos.idcliente == idclien && datos.Status=="PENDIENTE" select datos).FirstOrDefault();
                if (dato1 != null)
                {
                  
                     creditosdisponibles = Convert.ToDecimal(creditos1)-Convert.ToDecimal(dato1.Total);
                    if (creditosdisponibles > 0 && creditosdisponibles < Convert.ToDecimal(Total) || creditosdisponibles < 0)
                    {
                        mensaje = "La Factura Excede El límite de Crédito del Cliente el cual cuenta actualmente con " + "$" + creditosdisponibles.ToString("00");

                    }
                }
                

                else if (Convert.ToDecimal(creditos) > 0 && Convert.ToDecimal(creditos1) < Convert.ToDecimal(Total))
                {
                    mensaje = "La Factura Excede El límite de Crédito del Cliente el cual cuenta con " + "$" + creditos + " Aprobado";
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
                    factura.Status ="PENDIENTE";
                    factura.idcliente =Convert.ToInt32(idcliente);

                    db.facturas.Add(factura);
                    db.SaveChanges();
                    int id = factura.idfactura;


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
                        DetalleVenta objDetalleVenta = new DetalleVenta(id, idventa, idProducto, descripcion1, cantidad, precio1, descuento, itbis, subtotal, total, totaldescuento);
                        Session["idVenta"] = idventa;

                        db.DetalleVenta.Add(objDetalleVenta);
                        db.SaveChanges();

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
                factura.Status ="PAGADA";
                factura.idcliente = Convert.ToInt32(idcliente);

                db.facturas.Add(factura);
                db.SaveChanges();
                int id = factura.idfactura;


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
                    DetalleVenta objDetalleVenta = new DetalleVenta(id, idventa, idProducto, descripcion1, cantidad, precio1, descuento, itbis, subtotal, total, totaldescuento);
                    Session["idVenta"] = idventa;

                    db.DetalleVenta.Add(objDetalleVenta);
                    db.SaveChanges();

                }
                mensaje = "VENTA GUARDADA CON EXITO...";

            }
            return Json(mensaje);



        }
        // GET: facturas/Details/5
        public ActionResult Details(int? id)
        {
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
        public ActionResult reporteActual()
        {
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
        // GET: facturas/Create
        public ActionResult Create()
        {
            ViewBag.cliente = new SelectList(db.clientes.Where(m => m.Status == "1"), "idcliente", "nombre");
            ViewBag.vendedor = new SelectList(db.vendedores.Where(m => m.Status == "1"), "nombre", "nombre");
            var ListadoDetalle = new List<DetalleVenta>();
            ListadoDetalle.Add(new DetalleVenta());
            ListadoDetalle.Add(new DetalleVenta());
           
            ViewBag.producto = new SelectList(db.productos.Where(m => m.Status == "1"), "idProducto", "Descripcion");
            return View();
        }
        public JsonResult Getproducto(int idproducto)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<productos> productos = db.productos.Where(m => m.idProducto == idproducto && m.Status=="1").ToList();

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
