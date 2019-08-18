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
        public ActionResult PDf(string importe,List<DetalleVenta> ListadoDetalle)
        {
            string mensaje = "";

            facturas factura = new facturas();

            foreach (var data in ListadoDetalle)
            {
                string idProducto = data.idproducto.ToString();
                int cantidad = Convert.ToInt32(data.cantidad.ToString());
                decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                DetalleVenta objDetalleVenta = new DetalleVenta(001,001, Convert.ToInt32(idProducto),"hola", cantidad,200, descuento,"350", subtotal);
                db.DetalleVenta.Add(objDetalleVenta);
                db.SaveChanges();

            }
            mensaje = "VENTA GUARDADA CON EXITO...";
            string DirectorioReportesRelativo = "~/RTPFactura/";
            string urlArchivo = string.Format("{0}.{1}", "Report1", "rdlc");

            string FullPathReport = string.Format("{0}{1}",
                                    this.HttpContext.Server.MapPath(DirectorioReportesRelativo),
                                     urlArchivo);

            ReportViewer Reporte = new ReportViewer();

            Reporte.Reset();
            Reporte.LocalReport.ReportPath = FullPathReport;
            ReportDataSource DataSource = new ReportDataSource("MyDataset", ListadoDetalle);
            Reporte.LocalReport.DataSources.Add(DataSource);
            Reporte.LocalReport.Refresh();
            byte[] file = Reporte.LocalReport.Render("PDF");

            return File(new MemoryStream(file).ToArray(),
                      System.Net.Mime.MediaTypeNames.Application.Octet,
                      /*Esto para forzar la descarga del archivo*/
                      string.Format("{0}{1}", "archivoprueba.", "PDF"));


        }
        // GET: facturas/Details/5
        public ActionResult Details(int? id)
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

        // GET: facturas/Create
        public ActionResult Create()
        {
            ViewBag.cliente = new SelectList(db.clientes, "nombre", "nombre");
            ViewBag.vendedor = new SelectList(db.vendedores, "nombre", "nombre");
            var ListadoDetalle = new List<DetalleVenta>();
            ListadoDetalle.Add(new DetalleVenta());
            ListadoDetalle.Add(new DetalleVenta());
           
            ViewBag.producto = new SelectList(db.productos.Where(m => m.Status == "1"), "idProducto", "Descripcion");
            return View();
        }
        public JsonResult Getproducto(string idproducto)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<productos> productos = db.productos.Where(m => m.Descripcion == idproducto && m.Status=="1").ToList();

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

            foreach (var data in ListadoDetalle)
            {
                string idProducto = data.idproducto.ToString();
                int cantidad = Convert.ToInt32(data.cantidad.ToString());
                decimal descuento = Convert.ToDecimal(data.descuento.ToString());
                decimal subtotal = Convert.ToDecimal(data.importe.ToString());
                DetalleVenta objDetalleVenta = new DetalleVenta(001, 001, Convert.ToInt32(idProducto), "hola", cantidad, 200, descuento, "350", subtotal);
                db.DetalleVenta.Add(objDetalleVenta);

                db.SaveChanges();

            }
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
