using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
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
            var productos = db.productos.Include(p => p.tiposproductos).Where(m=>m.Status=="1");
            return View(productos.ToList());
        }
        public ActionResult DatosDetalle(string id)
        {

            return View(db.DetalleCompra.Where(m=>m.codproducto==id).ToList());
        }
        public ActionResult DatosMostrarInventario()
        {
            return View();
        }
        public JsonResult Getprod(int? idproducto)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Inventario> producto = db.Inventario.Where(m => m.idProducto==idproducto).ToList();
            ViewBag.deta = producto;
            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(producto, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Inventario()
        {
            return View(db.Inventario.Where(inventario=>inventario.status=="1").ToList());
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
                ViewBag.idtipo = new SelectList(db.tiposproductos, "idtipoproducto", "nombre", productos.idtipo);
                ViewBag.itbis = new SelectList(db.itbis, "valor", "valor", productos.itbis);
                ViewBag.invent = (from s in db.productos where s.idProducto == id select s.Inventario).FirstOrDefault();
            
                ViewBag.foto = productos.Foto;

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
        public ActionResult Create([Bind(Include = "idProducto,codigobarra,Descripcion,idtipo,Precio,itbis,costo,nota,Inventario,CodProducto,Foto")] productos productos)
        {

            var t = (from s in db.productos where s.idProducto==productos.idProducto select s.idProducto).Count();

            if (t != 0)
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
                        productos.Precio = Decimal.Parse("0.00");
                    }
                    if (productos.costo == null)
                    {
                        productos.costo = Decimal.Parse("0.00");
                    }
                    if ((from t1 in db.productos where t1.idProducto == productos.idProducto select t1.cantidad).FirstOrDefault()<=0)
                    {
                        productos.cantidad = 0;
                    }
                    else
                    {
                        productos.cantidad = (from t1 in db.productos where t1.idProducto == productos.idProducto select t1.cantidad).FirstOrDefault();
                    }
                  
                    
                    productos.Status = "1";
                    if (productos.Inventario == "si")
                    {
                        if ((from a in db.Inventario where a.idProducto == productos.idProducto select a).FirstOrDefault() != null)
                        {
                            var q = (from a in db.Inventario where a.idProducto == productos.idProducto select a).First();
                            q.idProducto = productos.idProducto;
                            q.itbis = productos.itbis;
                            q.nota = productos.nota;
                            q.Precio = productos.Precio;
                            q.costo = productos.costo;
                            q.codigodebarras = productos.codigobarra;
                            q.Descripcion = productos.Descripcion;
                            q.CodigoProducto = productos.CodProducto;
                            if (q.cantidad <=0)
                            {
                                q.cantidad = 0;
                            }
                            else
                            {
                                q.cantidad = (from t1 in db.productos where t1.idProducto == productos.idProducto select t1.cantidad).FirstOrDefault();
                            }
                           
                            q.Foto = productos.Foto;
                            q.status = productos.Status;
                            db.SaveChanges();
                        }
                        else if((from a in db.Inventario where a.idProducto == productos.idProducto select a).FirstOrDefault() == null)
                        {
                            if (productos.Inventario == "si")
                            {
                                Inventario invent = new Inventario();
                                invent.idProducto = productos.idProducto;
                                invent.itbis = productos.itbis;
                                invent.nota = productos.nota;
                                invent.Precio = productos.Precio;
                                invent.costo = productos.costo;
                                invent.codigodebarras = productos.codigobarra;
                                invent.Descripcion = productos.Descripcion;
                                invent.CodigoProducto = productos.CodProducto;
                                invent.Foto = productos.Foto;
                                invent.status = productos.Status;
                                if (invent.cantidad <= 0)
                                {
                                    invent.cantidad = 0;
                                }
                                else
                                {
                                    invent.cantidad = (from t1 in db.productos where t1.idProducto == productos.idProducto select t1.cantidad).FirstOrDefault();
                                }
                               
                                db.Inventario.Add(invent);
                            }
                        }
                    }
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
                        productos.Precio = Decimal.Parse("0.00");
                    }
                    if (productos.costo == null)
                    {
                        productos.costo = Decimal.Parse("0.00");
                    }
                    productos.Status = "1";
                    productos.cantidad = 0;
                    if (productos.Inventario == "si")
                    {
                        Inventario invent = new Inventario();
                        invent.idProducto = productos.idProducto;
                        invent.itbis = productos.itbis;
                        invent.nota = productos.nota;
                        invent.Precio = productos.Precio;
                        invent.costo = productos.costo;
                        invent.codigodebarras = productos.codigobarra;
                        invent.Descripcion = productos.Descripcion;
                        invent.CodigoProducto = productos.CodProducto;
                        invent.Foto = productos.Foto;
                        invent.status = productos.Status;
                        invent.cantidad = productos.cantidad;
                        db.Inventario.Add(invent);
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
            var inventario = (from s in db.Inventario where s.idProducto==id select s).FirstOrDefault();
            if (inventario != null)
            {
                inventario.status = "0";
            }
            productos.Status ="0";
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
