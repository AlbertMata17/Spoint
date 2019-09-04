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
    public class NcfTipoesController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: NcfTipoes
        public ActionResult Index()
        {
            return View(db.NcfTipo.ToList());
        }

        // GET: NcfTipoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NcfTipo ncfTipo = db.NcfTipo.Find(id);
            if (ncfTipo == null)
            {
                return HttpNotFound();
            }
            return View(ncfTipo);
        }

        // GET: NcfTipoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NcfTipoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipo,nombre,status")] NcfTipo ncfTipo)
        {
            if (ModelState.IsValid)
            {
                db.NcfTipo.Add(ncfTipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ncfTipo);
        }

        // GET: NcfTipoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NcfTipo ncfTipo = db.NcfTipo.Find(id);
            if (ncfTipo == null)
            {
                return HttpNotFound();
            }
            return View(ncfTipo);
        }

        // POST: NcfTipoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTipo,nombre,status")] NcfTipo ncfTipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ncfTipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ncfTipo);
        }

        // GET: NcfTipoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NcfTipo ncfTipo = db.NcfTipo.Find(id);
            if (ncfTipo == null)
            {
                return HttpNotFound();
            }
            return View(ncfTipo);
        }

        // POST: NcfTipoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NcfTipo ncfTipo = db.NcfTipo.Find(id);
            db.NcfTipo.Remove(ncfTipo);
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
