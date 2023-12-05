using Microsoft.AspNet.Identity;
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
    public class ReseñasController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();

        // GET: Reseñas
        public ActionResult Index()
        {
            var reseñas = db.Reseñas.Include(r => r.AspNetUsers);
            return View(reseñas.ToList());
        }

        // GET: Reseñas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseñas reseñas = db.Reseñas.Find(id);
            if (reseñas == null)
            {
                return HttpNotFound();
            }
            return View(reseñas);
        }

        // GET: Reseñas/Create
        public ActionResult Create()
        {
            ViewBag.Id_Usuario = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Reseñas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Resena,Comentario,Calificacion,Id_Usuario")] Reseñas reseñas)
        {

            if (ModelState.IsValid)
            {
                db.Reseñas.Add(reseñas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Usuario = new SelectList(db.AspNetUsers, "Id", "Email", reseñas.Id_Usuario);
            return View(reseñas);
        }

        // GET: Reseñas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseñas reseñas = db.Reseñas.Find(id);
            if (reseñas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Usuario = new SelectList(db.AspNetUsers, "Id", "Email", reseñas.Id_Usuario);
            return View(reseñas);
        }

        // POST: Reseñas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Resena,Comentario,Calificacion,Id_Usuario")] Reseñas reseñas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reseñas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Usuario = new SelectList(db.AspNetUsers, "Id", "Email", reseñas.Id_Usuario);
            return View(reseñas);
        }

        // GET: Reseñas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reseñas reseñas = db.Reseñas.Find(id);
            if (reseñas == null)
            {
                return HttpNotFound();
            }
            return View(reseñas);
        }

        // POST: Reseñas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reseñas reseñas = db.Reseñas.Find(id);
            db.Reseñas.Remove(reseñas);
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