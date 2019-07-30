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
            var suplidores = db.suplidores.Include(s => s.ciudad);
            return View(suplidores.ToList());
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
        public ActionResult Create()
        {
            ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre");
            return View();
        }

        // POST: suplidores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idSuplidor,nombre,telefono,direccion,idciudad,correo")] suplidores suplidores)
        {
            if (ModelState.IsValid)
            {
                db.suplidores.Add(suplidores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre", suplidores.idciudad);
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
            ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre", suplidores.idciudad);
            return View(suplidores);
        }

        // POST: suplidores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSuplidor,nombre,telefono,direccion,idciudad,correo")] suplidores suplidores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suplidores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idciudad = new SelectList(db.ciudad, "idciudad", "Nombre", suplidores.idciudad);
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
            db.suplidores.Remove(suplidores);
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
