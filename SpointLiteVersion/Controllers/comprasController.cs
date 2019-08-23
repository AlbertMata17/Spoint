﻿using System;
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
    public class comprasController : Controller
    {
        private spointEntities db = new spointEntities();

        // GET: compras
        public ActionResult Index()
        {
            return View(db.compras.ToList());
        }

        // GET: compras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compras compras = db.compras.Find(id);
            if (compras == null)
            {
                return HttpNotFound();
            }
            return View(compras);
        }

        // GET: compras/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                var m = 1;
                var l = (from r in db.compras select r.idcompra).Count();


                while (m <= l)
                {
                    m++;
                };

                ViewBag.producto = new SelectList(db.productos.Where(re=>re.Status=="1"), "idProducto", "Descripcion");
                ViewBag.suplidor = new SelectList(db.suplidores.Where(re => re.Status == "1"), "idSuplidor", "nombre");

                var codigo = "COMP000" + m;
                var o = (from n in db.compras where n.NoCompra == codigo select n).Count();
                if (o != 0)
                {
                    m = m + 1;
                    ViewBag.codigo = "COMP000" + m;
                }
                else
                {
                    ViewBag.codigo = codigo;
                }

                return View();
            }
            ViewBag.producto = new SelectList(db.productos.Where(re => re.Status == "1"), "idProducto", "Descripcion");
            ViewBag.suplidor = new SelectList(db.suplidores.Where(re => re.Status == "1"), "idSuplidor", "nombre");
            return View();
        }
        public JsonResult Getproducto(string idproducto)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<productos> productos = db.productos.Where(m => m.Descripcion == idproducto).ToList();

            //ViewBag.FK_Vehiculo = new SelectList(db.Vehiculo.Where(a => a.Clase == Clase && a.Estatus == "Disponible"), "VehiculoId", "Marca");
            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        // POST: compras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idcompra,NoCompra,Fecha,Observacion,Suplidor,Articulo,Cantidad")] compras compras)
        {
            if (ModelState.IsValid)
            {
                db.compras.Add(compras);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(compras);
        }

        // GET: compras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compras compras = db.compras.Find(id);
            if (compras == null)
            {
                return HttpNotFound();
            }
            return View(compras);
        }

        // POST: compras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idcompra,NoCompra,Fecha,Observacion,Suplidor,Articulo,Cantidad")] compras compras)
        {
            if (ModelState.IsValid)
            {
                db.Entry(compras).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(compras);
        }

        // GET: compras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            compras compras = db.compras.Find(id);
            if (compras == null)
            {
                return HttpNotFound();
            }
            return View(compras);
        }
        public ActionResult PDf(string fecha, string nocompra, string observacion, string suplidor, string Total, List<DetalleCompra> ListadoDetalle)
        {
            string mensaje = "";
            if (fecha == "" || observacion == "" || suplidor == "")
            {
                if (fecha == "") mensaje = "ERROR EN EL CAMPO FECHA";
                if (suplidor == "") mensaje = "ERROR CON EL SUPLIDOR";
             


            }
            else
            {
                if (Total == "") Total = "0.00";
                int id1 = 0;
                DetalleCompra detalle = new DetalleCompra();
                var verificar = (from s in db.DetalleCompra select s.idDetalle);
                if (verificar.Count() > 0)
                {
                    id1 = (from s in db.DetalleCompra select s.idDetalle).Max();
                }
                int idventa = id1 + 1;
                compras compras = new compras();
                compras.Suplidor = suplidor;
                compras.Fecha = Convert.ToDateTime(fecha);
                compras.Observacion = observacion.ToUpper();
                compras.NoCompra = nocompra;
                compras.Total = Convert.ToDecimal(Total);

                db.compras.Add(compras);
                db.SaveChanges();
                int id = compras.idcompra;

                productos producto = new productos();
                foreach (var data in ListadoDetalle)
                {
                    string idProducto = data.codproducto.ToString();
                    int cantidad = Convert.ToInt32(data.cantidad.ToString());
                    if (idProducto != "")
                    {
                        var q = (from a in db.productos where a.CodProducto == idProducto select a).First();
                        q.cantidad += cantidad;
                        var qs = (from a in db.Inventario where a.CodigoProducto == idProducto select a).First();
                        qs.cantidad += cantidad;
                        qs.Tipo = "Compra";
                        qs.Fecha = Convert.ToDateTime(fecha);
                        db.SaveChanges();
                    }
                    decimal total = Convert.ToDecimal(data.importe.ToString());
                    string descripcion1 = data.descripcion.ToString();
                    decimal precio1 = Convert.ToDecimal(data.costo.ToString());
                    DetalleCompra objDetalleVenta = new DetalleCompra(idProducto, id, cantidad, descripcion1, precio1, total, Convert.ToDateTime(fecha));
                    Session["idVenta"] = idventa;

                    db.DetalleCompra.Add(objDetalleVenta);
                    db.SaveChanges();

                }
                mensaje = "COMPRA GUARDADA CON EXITO...";

            }
            return Json(mensaje);



        }

        // POST: compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            compras compras = db.compras.Find(id);
            db.compras.Remove(compras);
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
