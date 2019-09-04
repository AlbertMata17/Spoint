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
    public class NCFs1Controller : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: NCFs1
        public ActionResult Index()
        {
            var nCF = db.NCF.Include(n => n.Empresa).Include(n => n.Login);
            return View(nCF.ToList());
        }

        // GET: NCFs1/Details/5
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

        // GET: NCFs1/Create
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
                ViewBag.empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre");
                ViewBag.usuarioid = new SelectList(db.Login, "LoginId", "Username");
                return View();
            }
            NCF ncf = db.NCF.Find(id);
            if (ncf == null)
            {
                return HttpNotFound();
            }
            if (id != null)
            {
                ViewBag.empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre");
                ViewBag.usuarioid = new SelectList(db.Login, "LoginId", "Username");
                ViewBag.id = "algo";

                return View(ncf);
            }
            return View();

        }

        // POST: NCFs1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idNCF,NombreComp,Estatus,empresaid,usuarioid,Descripcion,tipodocumento")] NCF nCF)
        {
            var t = (from s in db.NCF where s.idNCF == nCF.idNCF select s.idNCF).Count();
            if (t != 0)
            {
                if (ModelState.IsValid)
                {
                  

                    if (nCF.NombreComp != null)
                    {
                        nCF.NombreComp = nCF.NombreComp.ToUpper();
                    }
                    if (nCF.Descripcion != null)
                    {
                        nCF.Descripcion = nCF.Descripcion.ToUpper();
                    }
                    nCF.Estatus = "1";
                    var usuarioid = Session["userid"].ToString();
                    var empresaid = Session["empresaid"].ToString();
                    nCF.usuarioid = Convert.ToInt32(usuarioid);
                    nCF.empresaid = Convert.ToInt32(empresaid);
                    db.Entry(nCF).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (nCF.idNCF <= 0)
            {
                if (ModelState.IsValid)
                {
                    if (nCF.NombreComp != null)
                    {
                        nCF.NombreComp = nCF.NombreComp.ToUpper();
                    }
                    if (nCF.Descripcion != null)
                    {
                        nCF.Descripcion = nCF.Descripcion.ToUpper();
                    }
                    nCF.Estatus = "1";
                    nCF.tipodocumento = 1;
                    var usuarioid = Session["userid"].ToString();
                    var empresaid = Session["empresaid"].ToString();
                    nCF.usuarioid = Convert.ToInt32(usuarioid);
                    nCF.empresaid = Convert.ToInt32(empresaid);
                    db.NCF.Add(nCF);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                }

                ViewBag.empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", nCF.empresaid);
            ViewBag.usuarioid = new SelectList(db.Login, "LoginId", "Username", nCF.usuarioid);
            return View(nCF);
        }

        // GET: NCFs1/Edit/5
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
            ViewBag.empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", nCF.empresaid);
            ViewBag.usuarioid = new SelectList(db.Login, "LoginId", "Username", nCF.usuarioid);
            return View(nCF);
        }

        // POST: NCFs1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idNCF,NombreComp,Estatus,empresaid,usuarioid,Descripcion,tipodocumento")] NCF nCF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nCF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.empresaid = new SelectList(db.Empresa, "IdEmpresa", "Nombre", nCF.empresaid);
            ViewBag.usuarioid = new SelectList(db.Login, "LoginId", "Username", nCF.usuarioid);
            return View(nCF);
        }

        // GET: NCFs1/Delete/5
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

        // POST: NCFs1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NCF nCF = db.NCF.Find(id);
            var q = (from a in db.NCF where a.idNCF == id select a).First();
            q.Estatus = "0";
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
