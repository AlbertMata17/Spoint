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
    public class suplidoresController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: suplidores
        public ActionResult Index()
        {

            return View(db.suplidores.Where(m=>m.Status=="1").ToList());
        }

        // GET: suplidores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            suplidores suplidores = db.suplidores.Find(id);
            if (suplidores == null)
            {
                return HttpNotFound();
            }
            return View(suplidores);
        }

        // GET: suplidores/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return View();

            }
            suplidores suplidores = db.suplidores.Find(id);
            if (suplidores == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.id = "algo";
                ViewBag.foto = suplidores.Foto;

                return View(suplidores);
            }

            return View();
        }

        // POST: suplidores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idSuplidor,nombre,telefono,direccion,correo,cedula,Foto")] suplidores suplidores)
        {
            var t = (from s in db.suplidores where s.idSuplidor == suplidores.idSuplidor select s.idSuplidor).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    if (suplidores.nombre != null)
                    {
                        suplidores.nombre = suplidores.nombre.ToUpper();
                    }
                    if (suplidores.direccion != null)
                    {
                        suplidores.direccion = suplidores.direccion.ToUpper();
                    }
                    if (suplidores.correo != null)
                    {
                        suplidores.correo = suplidores.correo.ToUpper();
                    }
                    suplidores.Status = "1";

                    db.Entry(suplidores).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (t <= 0)
            {
                if (ModelState.IsValid)
                {
                    if (suplidores.nombre != null)
                    {
                        suplidores.nombre = suplidores.nombre.ToUpper();
                    }
                    if (suplidores.direccion != null)
                    {
                        suplidores.direccion = suplidores.direccion.ToUpper();
                    }
                    if (suplidores.correo != null)
                    {
                        suplidores.correo = suplidores.correo.ToUpper();
                    }
                    suplidores.Status = "1";

                    db.suplidores.Add(suplidores);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(suplidores);
        }

        // GET: suplidores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            suplidores suplidores = db.suplidores.Find(id);
            if (suplidores == null)
            {
                return HttpNotFound();
            }
            return View(suplidores);
        }

        // POST: suplidores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSuplidor,nombre,telefono,direccion,correo,cedula")] suplidores suplidores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suplidores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suplidores);
        }

        // GET: suplidores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            suplidores suplidores = db.suplidores.Find(id);
            if (suplidores == null)
            {
                return HttpNotFound();
            }
            return View(suplidores);
        }

        // POST: suplidores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            suplidores suplidores = db.suplidores.Find(id);
            suplidores.Status = "0";

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
