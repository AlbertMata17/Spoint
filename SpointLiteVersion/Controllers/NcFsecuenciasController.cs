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
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            return View(db.NcFsecuencia.Where(m=>m.status==1 && m.empresaid==empresaid1).ToList());
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
        public ActionResult Create(int? id)
        {


            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            if (id == null)
            {

                ViewBag.ncf = new SelectList(db.NCF.Where(m => m.empresaid == empresaid1), "idNCF", "NombreComp");
                return View();
            }
            NcFsecuencia cnfsecuencia = db.NcFsecuencia.Find(id);
            if (cnfsecuencia == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.ncf = new SelectList(db.NCF.Where(m => m.empresaid == empresaid1), "idNCF", "NombreComp",cnfsecuencia.idncf);

                ViewBag.id = "algo";

                return View(cnfsecuencia);
            }
            return View();
        }

        // POST: NcFsecuencias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDetalle,fecha,fechavence,prefijo,idncf,desde,hasta,usado,autorizacion,status,empresaid,usuarioid")] NcFsecuencia ncFsecuencia)
        {
            var usuarioid12 = Session["userid"].ToString();
            var empresaid12 = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid12);
            var empresaid1 = Convert.ToInt32(empresaid12);
            var t = (from s in db.NcFsecuencia where s.idDetalle == ncFsecuencia.idDetalle select s.idDetalle).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                    if (ncFsecuencia.prefijo != null)
                    {
                        ncFsecuencia.prefijo = ncFsecuencia.prefijo.ToUpper();
                    }
                    ncFsecuencia.status = 1;
                    var usuarioid = Session["userid"].ToString();
                    var empresaid = Session["empresaid"].ToString();
                    ncFsecuencia.usuarioid = Convert.ToInt32(usuarioid);
                    ncFsecuencia.empresaid = Convert.ToInt32(empresaid);
                    ncFsecuencia.usado = 0;
                    db.Entry(ncFsecuencia).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (ncFsecuencia.idDetalle <= 0)
            {
                if (ModelState.IsValid)
                {
                    if (ncFsecuencia.prefijo != null)
                    {
                        ncFsecuencia.prefijo = ncFsecuencia.prefijo.ToUpper();
                    }
                    ncFsecuencia.status = 1;
                    var usuarioid = Session["userid"].ToString();
                    var empresaid = Session["empresaid"].ToString();
                    ncFsecuencia.usuarioid = Convert.ToInt32(usuarioid);
                    ncFsecuencia.empresaid = Convert.ToInt32(empresaid);
                    ncFsecuencia.usado = 0;
                    db.NcFsecuencia.Add(ncFsecuencia);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }


            ViewBag.ncf = new SelectList(db.NCF.Where(m => m.empresaid == empresaid1), "idNCF", "NombreComp",ncFsecuencia.idncf);

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
            NcFsecuencia ncfsecuencia = db.NcFsecuencia.Find(id);
            ncfsecuencia.status = 0;
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
