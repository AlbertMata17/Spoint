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
    public class tiposproductosController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: tiposproductos
        public ActionResult Index()
        {
            return View(db.tiposproductos.ToList());
        }

        // GET: tiposproductos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tiposproductos tiposproductos = db.tiposproductos.Find(id);
            if (tiposproductos == null)
            {
                return HttpNotFound();
            }
            return View(tiposproductos);
        }

        // GET: tiposproductos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tiposproductos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idtipoproducto,nombre")] tiposproductos tiposproductos)
        {
            if (ModelState.IsValid)
            {
                var usuarioid = Session["userid"].ToString();
                var empresaid = Session["empresaid"].ToString();
                tiposproductos.usuarioid = Convert.ToInt32(usuarioid);
                tiposproductos.empresaid = Convert.ToInt32(empresaid);
                db.tiposproductos.Add(tiposproductos);
                db.SaveChanges();
            }

            return new ContentResult() { Content = "Recepción satisfactoria" };
        }

        // GET: tiposproductos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tiposproductos tiposproductos = db.tiposproductos.Find(id);
            if (tiposproductos == null)
            {
                return HttpNotFound();
            }
            return View(tiposproductos);
        }

        // POST: tiposproductos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idtipoproducto,nombre")] tiposproductos tiposproductos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposproductos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposproductos);
        }

        // GET: tiposproductos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tiposproductos tiposproductos = db.tiposproductos.Find(id);
            if (tiposproductos == null)
            {
                return HttpNotFound();
            }
            return View(tiposproductos);
        }

        // POST: tiposproductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tiposproductos tiposproductos = db.tiposproductos.Find(id);
            db.tiposproductos.Remove(tiposproductos);
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
