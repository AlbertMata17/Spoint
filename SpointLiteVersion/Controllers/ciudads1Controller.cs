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
    public class ciudads1Controller : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: ciudads1
        public ActionResult Index()
        {
            return View(db.ciudad.ToList());
        }

        // GET: ciudads1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciudad ciudad = db.ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // GET: ciudads1/Create
        public ActionResult Create()
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            return View();
        }

        // POST: ciudads1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idciudad,Nombre")] ciudad ciudad)
        {
            if (ModelState.IsValid)
            {
                var usuarioid = Session["userid"].ToString();
                var empresaid = Session["empresaid"].ToString();
                ciudad.usuarioid = Convert.ToInt32(usuarioid);
                ciudad.empresaid = Convert.ToInt32(empresaid);
                ciudad.Nombre = ciudad.Nombre.ToUpper();
                db.ciudad.Add(ciudad);
                db.SaveChanges();
            }

            return new ContentResult() { Content = "Recepción satisfactoria" };
        }

        // GET: ciudads1/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciudad ciudad = db.ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: ciudads1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idciudad,Nombre")] ciudad ciudad)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (ModelState.IsValid)
            {
                ciudad.Nombre = ciudad.Nombre.ToUpper();
                db.Entry(ciudad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ciudad);
        }

        // GET: ciudads1/Delete/5
        public ActionResult Delete(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ciudad ciudad = db.ciudad.Find(id);
            if (ciudad == null)
            {
                return HttpNotFound();
            }
            return View(ciudad);
        }

        // POST: ciudads1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ciudad ciudad = db.ciudad.Find(id);
            db.ciudad.Remove(ciudad);
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
