using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpointLiteVersion.Models;

namespace SpointLiteVersion.Controllers
{
    public class productos1Controller : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: productos1
        public ActionResult Index()
        {
            var productos = db.productos.Include(p => p.tiposproductos);
            return View(productos.ToList());
        }

        public ActionResult Inventario()
        {
            var productos = db.productos.Include(p => p.tiposproductos).Where(m => m.Inventario == "si");
            return View(productos.ToList());
        }
        public JsonResult Getproducto()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<tiposproductos> producto = db.tiposproductos.ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(producto, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getdatos(string CodProducto)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<productos> producto = db.productos.Where(m => m.CodProducto == CodProducto).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(producto, JsonRequestBehavior.AllowGet);
        }
        // GET: productos1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productos productos = db.productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // GET: productos1/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                var m = 1;
                var l = (from r in db.productos select r.idProducto).Count();


                while (m <= l) {
                        m++;
                    };
                
                
                ViewBag.idtipo = new SelectList(db.tiposproductos, "idtipoproducto", "nombre");
                ViewBag.itbis = new SelectList(db.itbis, "valor", "valor");
                var codigo= "PROD000" + m;
                var o = (from n in db.productos where n.CodProducto == codigo select n).Count();
                if (o != 0)
                {
                   m= m + 1;
                    ViewBag.codigo = "PROD000"+m;
                }
                else
                {
                    ViewBag.codigo = codigo;
                }
                return View();
            }
            productos productos = db.productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }

            if (id != null)
            {
                ViewBag.idtipo = new SelectList(db.tiposproductos, "idtipoproducto", "nombre",productos.idtipo);
                ViewBag.itbis = new SelectList(db.itbis, "valor", "valor",productos.itbis);
               

                ViewBag.id = "algo";
                return View(productos);
            }
            
            return View();
        }

        // POST: productos1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProducto,codigobarra,Descripcion,idtipo,Precio,itbis,costo,nota,Inventario,CodProducto")] productos productos)
        {

            var t = (from s in db.productos where s.idProducto==productos.idProducto select s.idProducto).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    
                    db.Entry(productos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if(productos.idProducto<=0)
            {

                if (ModelState.IsValid)
                {
                    if (productos.Descripcion != null)
                    {
                        productos.Descripcion = productos.Descripcion.ToUpper();
                    }
                    if (productos.nota != null)
                    {
                        productos.nota = productos.nota.ToUpper();
                    }
                    if (productos.Precio == null)
                    {
                        productos.Precio = "0.00";
                    }
                    if (productos.costo == null)
                    {
                        productos.costo = Decimal.Parse("0.00");
                    }
                    db.productos.Add(productos);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.idtipo = new SelectList(db.tiposproductos, "idtipoproducto", "nombre", productos.idtipo);
            ViewBag.itbis = new SelectList(db.itbis, "valor", "valor", productos.itbis);
            return View(productos);
        }

        // GET: productos1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productos productos = db.productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            ViewBag.idtipo = new SelectList(db.tiposproductos, "idtipoproducto", "nombre", productos.idtipo);
            return View(productos);
        }

        // POST: productos1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProducto,codigobarra,Descripcion,idtipo,Precio,itbis,costo,nota,Inventario")] productos productos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idtipo = new SelectList(db.tiposproductos, "idtipoproducto", "nombre", productos.idtipo);
            return View(productos);
        }

        // GET: productos1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productos productos = db.productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: productos1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            productos productos = db.productos.Find(id);
            db.productos.Remove(productos);
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
