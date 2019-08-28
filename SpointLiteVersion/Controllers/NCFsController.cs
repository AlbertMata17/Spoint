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
    public class NCFsController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: NCFs
        public ActionResult Index()
        {
            return View(db.NCF.ToList());
        }

        // GET: NCFs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NCF nCF = db.NCF.Find(id);
            if (nCF == null)
            {
                return HttpNotFound();
            }
            return View(nCF);
        }

        // GET: NCFs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NCFs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idNCF,Tipo,Estatus,NoNCF")] NCF nCF)
        {
            if (ModelState.IsValid)
            {
                db.NCF.Add(nCF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nCF);
        }

        // GET: NCFs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NCF nCF = db.NCF.Find(id);
            if (nCF == null)
            {
                return HttpNotFound();
            }
            return View(nCF);
        }

        // POST: NCFs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idNCF,Tipo,Estatus,NoNCF")] NCF nCF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nCF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nCF);
        }

        // GET: NCFs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NCF nCF = db.NCF.Find(id);
            if (nCF == null)
            {
                return HttpNotFound();
            }
            return View(nCF);
        }

        // POST: NCFs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NCF nCF = db.NCF.Find(id);
            db.NCF.Remove(nCF);
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
