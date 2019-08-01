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
    public class vendedoresController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: vendedores
        public ActionResult Index()
        {
            return View(db.vendedores.ToList());
        }

        // GET: vendedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vendedores vendedores = db.vendedores.Find(id);
            if (vendedores == null)
            {
                return HttpNotFound();
            }
            return View(vendedores);
        }

        // GET: vendedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: vendedores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idvendedor,nombre,direccion,telefono,cedula,cumpleaños,correo")] vendedores vendedores)
        {
            if (ModelState.IsValid)
            {
                db.vendedores.Add(vendedores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendedores);
        }

        // GET: vendedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vendedores vendedores = db.vendedores.Find(id);
            if (vendedores == null)
            {
                return HttpNotFound();
            }
            return View(vendedores);
        }

        // POST: vendedores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idvendedor,nombre,direccion,telefono,cedula,cumpleaños,correo")] vendedores vendedores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendedores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendedores);
        }

        // GET: vendedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vendedores vendedores = db.vendedores.Find(id);
            if (vendedores == null)
            {
                return HttpNotFound();
            }
            return View(vendedores);
        }

        // POST: vendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            vendedores vendedores = db.vendedores.Find(id);
            db.vendedores.Remove(vendedores);
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
