using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GananciaDiarias1Controller : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();

        // GET: GananciaDiarias1
        public ActionResult Index()
        {
            return View(db.GananciaDiaria.ToList());
        }

        // GET: GananciaDiarias1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GananciaDiaria gananciaDiaria = db.GananciaDiaria.Find(id);
            if (gananciaDiaria == null)
            {
                return HttpNotFound();
            }
            return View(gananciaDiaria);
        }

        // GET: GananciaDiarias1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GananciaDiarias1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Ganancias,Ingresos,Fecha,Egresos")] GananciaDiaria gananciaDiaria)
        {
            if (ModelState.IsValid)
            {
                db.GananciaDiaria.Add(gananciaDiaria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gananciaDiaria);
        }

        // GET: GananciaDiarias1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GananciaDiaria gananciaDiaria = db.GananciaDiaria.Find(id);
            if (gananciaDiaria == null)
            {
                return HttpNotFound();
            }
            return View(gananciaDiaria);
        }

        // POST: GananciaDiarias1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Ganancias,Ingresos,Fecha,Egresos")] GananciaDiaria gananciaDiaria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gananciaDiaria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gananciaDiaria);
        }

        // GET: GananciaDiarias1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GananciaDiaria gananciaDiaria = db.GananciaDiaria.Find(id);
            if (gananciaDiaria == null)
            {
                return HttpNotFound();
            }
            return View(gananciaDiaria);
        }

        // POST: GananciaDiarias1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GananciaDiaria gananciaDiaria = db.GananciaDiaria.Find(id);
            db.GananciaDiaria.Remove(gananciaDiaria);
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
