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
    public class TipoSuplidorsController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: TipoSuplidors
        public ActionResult Index()
        {
            return View(db.TipoSuplidor.ToList());
        }

        // GET: TipoSuplidors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoSuplidor tipoSuplidor = db.TipoSuplidor.Find(id);
            if (tipoSuplidor == null)
            {
                return HttpNotFound();
            }
            return View(tipoSuplidor);
        }

        // GET: TipoSuplidors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoSuplidors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTipoSuplidor,Nombre")] TipoSuplidor tipoSuplidor)
        {
            if (ModelState.IsValid)
            {
                db.TipoSuplidor.Add(tipoSuplidor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoSuplidor);
        }

        // GET: TipoSuplidors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoSuplidor tipoSuplidor = db.TipoSuplidor.Find(id);
            if (tipoSuplidor == null)
            {
                return HttpNotFound();
            }
            return View(tipoSuplidor);
        }

        // POST: TipoSuplidors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTipoSuplidor,Nombre")] TipoSuplidor tipoSuplidor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoSuplidor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoSuplidor);
        }

        // GET: TipoSuplidors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoSuplidor tipoSuplidor = db.TipoSuplidor.Find(id);
            if (tipoSuplidor == null)
            {
                return HttpNotFound();
            }
            return View(tipoSuplidor);
        }

        // POST: TipoSuplidors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoSuplidor tipoSuplidor = db.TipoSuplidor.Find(id);
            db.TipoSuplidor.Remove(tipoSuplidor);
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
