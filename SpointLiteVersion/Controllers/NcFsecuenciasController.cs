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
    public class NcFsecuenciasController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: NcFsecuencias
        public ActionResult Index()
        {
            return View(db.NcFsecuencia.ToList());
        }

        // GET: NcFsecuencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NcFsecuencia ncFsecuencia = db.NcFsecuencia.Find(id);
            if (ncFsecuencia == null)
            {
                return HttpNotFound();
            }
            return View(ncFsecuencia);
        }

        // GET: NcFsecuencias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NcFsecuencias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDetalle,fecha,fechavence,prefijo,idncf,desde,hasta,usado,autorizacion,status,empresaid,usuarioid")] NcFsecuencia ncFsecuencia)
        {
            if (ModelState.IsValid)
            {
                db.NcFsecuencia.Add(ncFsecuencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ncFsecuencia);
        }

        // GET: NcFsecuencias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NcFsecuencia ncFsecuencia = db.NcFsecuencia.Find(id);
            if (ncFsecuencia == null)
            {
                return HttpNotFound();
            }
            return View(ncFsecuencia);
        }

        // POST: NcFsecuencias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDetalle,fecha,fechavence,prefijo,idncf,desde,hasta,usado,autorizacion,status,empresaid,usuarioid")] NcFsecuencia ncFsecuencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ncFsecuencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ncFsecuencia);
        }

        // GET: NcFsecuencias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NcFsecuencia ncFsecuencia = db.NcFsecuencia.Find(id);
            if (ncFsecuencia == null)
            {
                return HttpNotFound();
            }
            return View(ncFsecuencia);
        }

        // POST: NcFsecuencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NcFsecuencia ncFsecuencia = db.NcFsecuencia.Find(id);
            db.NcFsecuencia.Remove(ncFsecuencia);
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
