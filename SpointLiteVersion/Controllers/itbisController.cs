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
    public class itbisController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: itbis
        public ActionResult Index()
        {
            return View(db.itbis.ToList());
        }

        // GET: itbis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itbis itbis = db.itbis.Find(id);
            if (itbis == null)
            {
                return HttpNotFound();
            }
            return View(itbis);
        }

        // GET: itbis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: itbis/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,valor")] itbis itbis)
        {
            if (ModelState.IsValid)
            {
                db.itbis.Add(itbis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(itbis);
        }

        // GET: itbis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itbis itbis = db.itbis.Find(id);
            if (itbis == null)
            {
                return HttpNotFound();
            }
            return View(itbis);
        }

        // POST: itbis/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,valor")] itbis itbis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itbis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itbis);
        }

        // GET: itbis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itbis itbis = db.itbis.Find(id);
            if (itbis == null)
            {
                return HttpNotFound();
            }
            return View(itbis);
        }

        // POST: itbis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            itbis itbis = db.itbis.Find(id);
            db.itbis.Remove(itbis);
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
