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
    public class suplidores1Controller : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: suplidores1
        public ActionResult Index()
        {
            return View(db.suplidores.ToList());
        }

        // GET: suplidores1/Details/5
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

        // GET: suplidores1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: suplidores1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idSuplidor,nombre,telefono,direccion,correo,cedula,Foto,Status")] suplidores suplidores)
        {
            if (ModelState.IsValid)
            {
                var usuarioid = Session["userid"].ToString();
                var empresaid = Session["empresaid"].ToString();
                suplidores.usuarioid = Convert.ToInt32(usuarioid);
                suplidores.empresaid = Convert.ToInt32(empresaid);
                db.suplidores.Add(suplidores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(suplidores);
        }

        // GET: suplidores1/Edit/5
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

        // POST: suplidores1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSuplidor,nombre,telefono,direccion,correo,cedula,Foto,Status")] suplidores suplidores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(suplidores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suplidores);
        }

        // GET: suplidores1/Delete/5
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

        // POST: suplidores1/Delete/5
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
