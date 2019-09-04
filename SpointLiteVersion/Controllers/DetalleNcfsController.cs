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
    public class DetalleNcfsController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: DetalleNcfs
        public ActionResult Index()
        {
            return View(db.DetalleNcf.ToList());
        }

        // GET: DetalleNcfs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleNcf detalleNcf = db.DetalleNcf.Find(id);
            if (detalleNcf == null)
            {
                return HttpNotFound();
            }
            return View(detalleNcf);
        }

        // GET: DetalleNcfs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DetalleNcfs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDetalle,idNcf,iddocumento,tipodocumento,secuencia,status,empresaid,usuarioid,idncfsecuencia")] DetalleNcf detalleNcf)
        {
            if (ModelState.IsValid)
            {
                db.DetalleNcf.Add(detalleNcf);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(detalleNcf);
        }

        // GET: DetalleNcfs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleNcf detalleNcf = db.DetalleNcf.Find(id);
            if (detalleNcf == null)
            {
                return HttpNotFound();
            }
            return View(detalleNcf);
        }

        // POST: DetalleNcfs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDetalle,idNcf,iddocumento,tipodocumento,secuencia,status,empresaid,usuarioid,idncfsecuencia")] DetalleNcf detalleNcf)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleNcf).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(detalleNcf);
        }

        // GET: DetalleNcfs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleNcf detalleNcf = db.DetalleNcf.Find(id);
            if (detalleNcf == null)
            {
                return HttpNotFound();
            }
            return View(detalleNcf);
        }

        // POST: DetalleNcfs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleNcf detalleNcf = db.DetalleNcf.Find(id);
            db.DetalleNcf.Remove(detalleNcf);
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
