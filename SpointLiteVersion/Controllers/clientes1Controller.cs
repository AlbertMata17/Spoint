using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SpointLiteVersion.Models;

namespace SpointLiteVersion.Controllers
{
    public class clientes1Controller : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: clientes1
        public ActionResult Index()
        {
            var clientes = db.clientes.Include(c => c.ciudad);
            return View(clientes.ToList());
        }

        // GET: clientes1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clientes clientes = db.clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }
        public JsonResult Getciudad()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ciudad> ciudad = db.ciudad.ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(ciudad, JsonRequestBehavior.AllowGet);
        }
        // GET: clientes1/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre");
                return View();

            }
            clientes clientes = db.clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }

            if (id != null)
            {
                ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre", clientes.idciudad);
                ViewBag.id = "algo";
                ViewBag.foto = clientes.Foto;

                return View(clientes);
            }
            return View();
        }

        // POST: clientes1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCliente,nombre,telefono,telefono2,idciudad,creditoaprobado,cedula,direccion,email,fechanacimiento,NCF,Observaciones,LimiteTiempo,Foto")] clientes clientes)
        {
            var t = (from s in db.clientes where s.idCliente == clientes.idCliente select s.idCliente).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    if (clientes.direccion != null)
                    {
                        clientes.direccion = clientes.direccion.ToUpper();
                    }
                    if (clientes.nombre != null)
                    {
                        clientes.nombre = clientes.nombre.ToUpper();
                    }
                    if (clientes.email != null)
                    {
                        clientes.email = clientes.email.ToUpper();
                    }
                    if (clientes.Observaciones != null)
                    {
                        clientes.Observaciones = clientes.Observaciones.ToUpper();
                    }
                    clientes.Status = "1";

                    db.Entry(clientes).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (clientes.idCliente <= 0)
            {
                if (ModelState.IsValid)
                {
                    if (clientes.direccion != null)
                    {
                        clientes.direccion = clientes.direccion.ToUpper();
                    }
                    if (clientes.nombre != null)
                    {
                        clientes.nombre = clientes.nombre.ToUpper();
                    }
                    if (clientes.email != null)
                    {
                        clientes.email = clientes.email.ToUpper();
                    }
                    if (clientes.Observaciones != null)
                    {
                        clientes.Observaciones = clientes.Observaciones.ToUpper();
                    }
                    clientes.Status = "1";
                    db.clientes.Add(clientes);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre", clientes.idciudad);
            return View(clientes);
        }

        // GET: clientes1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clientes clientes = db.clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre", clientes.idciudad);
            return View(clientes);
        }

        // POST: clientes1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCliente,nombre,telefono,telefono2,idciudad,creditoaprobado,cedula,direccion,email,fechanacimiento,NCF,Observaciones,LimiteTiempo")] clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre", clientes.idciudad);
            return View(clientes);
        }

        // GET: clientes1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            clientes clientes = db.clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        // POST: clientes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            clientes clientes = db.clientes.Find(id);
            clientes.Status = "0";
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
