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
        public ActionResult VistaPdf(string idproducto, string cantidad)
        {
            ViewBag.producto = idproducto;
            ViewBag.cantidad = cantidad;

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
        public ActionResult PDf(List<string> ListadoDetalle)
        {
            Console.Write(ListadoDetalle);
            ViewBag.producto = ListadoDetalle;
            ViewBag.cantidad = "hola";
          
            return new ViewAsPdf("VistaPdf",new { ViewBag.producto, ViewBag.cantidad });
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
        public ActionResult Create([Bind(Include = "idfactura,fecha,observacion,cliente,vendedor,producto,descripcion,precio,credito")] facturas facturas)
        {
            if (ModelState.IsValid)
            {
                db.facturas.Add(facturas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facturas);
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
