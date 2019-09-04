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

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

            return View(db.NCF.ToList());
        }

        // GET: NCFs/Details/5
        public ActionResult Details(int? id)
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

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


        public JsonResult GetNcf(int idproducto)
        {
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            db.Configuration.ProxyCreationEnabled = false;
            List<NCF> productos = db.NCF.Where(m => m.idNCF == idproducto && m.Estatus == "1" && m.empresaid == empresaid1).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(productos, JsonRequestBehavior.AllowGet);
        }
        // GET: NCFs/Create
        public ActionResult Create()
        {

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }
            var usuarioid = Session["userid"].ToString();
            var empresaid = Session["empresaid"].ToString();
            var usuarioid1 = Convert.ToInt32(usuarioid);
            var empresaid1 = Convert.ToInt32(empresaid);
            ViewBag.ncf = new SelectList(db.NCF.Where(m => m.Estatus == "1" && m.empresaid == empresaid1), "idNCF", "NombreComp");

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
                var usuarioid = Session["userid"].ToString();
                var empresaid = Session["empresaid"].ToString();
                nCF.usuarioid = Convert.ToInt32(usuarioid);
                nCF.empresaid = Convert.ToInt32(empresaid);
                db.NCF.Add(nCF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nCF);
        }

        // GET: NCFs/Edit/5
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

            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Logins");
            }

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
