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
    
    public class CitasController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();


        [Authorize(Roles = "Administrador")]
        // GET: Citas
        public ActionResult Index()
        {
            var citas = db.Citas.Include(c => c.AspNetUsers).Include(c => c.Empleados).Include(c => c.Servicios);
            return View(citas.ToList());
        }


        [Authorize(Roles = "Administrador")]
        // GET: Citas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citas citas = db.Citas.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            return View(citas);
        }



        [Authorize]
        // GET: Citas/Create
        public ActionResult Create()
        {
           
            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado");
            ViewBag.Id_Servicio = new SelectList(db.Servicios, "Id_Servicio", "Nombre_Servicio");
            return View();
        }




        // POST: Citas/Create
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Cita,Fecha_Cita,Hora_Cita,Id_Usuario,Id_Servicio,Id_Empleado")] Citas citas)
        {
            if (ModelState.IsValid)
            {
          
                string userId = User.Identity.GetUserId(); // 

                citas.Id_Usuario = userId;

                db.Citas.Add(citas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado", citas.Id_Empleado);
            ViewBag.Id_Servicio = new SelectList(db.Servicios, "Id_Servicio", "Nombre_Servicio", citas.Id_Servicio);

            return View(citas);
        }


        [Authorize(Roles = "Administrador")]
        // GET: Citas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citas citas = db.Citas.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Usuario = new SelectList(db.AspNetUsers, "Id", "Email", citas.Id_Usuario);
            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado", citas.Id_Empleado);
            ViewBag.Id_Servicio = new SelectList(db.Servicios, "Id_Servicio", "Nombre_Servicio", citas.Id_Servicio);
            return View(citas);
        }

        // POST: Citas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "Id_Cita,Fecha_Cita,Hora_Cita,Id_Usuario,Id_Servicio,Id_Empleado")] Citas citas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Usuario = new SelectList(db.AspNetUsers, "Id", "Email", citas.Id_Usuario);
            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado", citas.Id_Empleado);
            ViewBag.Id_Servicio = new SelectList(db.Servicios, "Id_Servicio", "Nombre_Servicio", citas.Id_Servicio);
            return View(citas);
        }

        // GET: Citas/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Citas citas = db.Citas.Find(id);
            if (citas == null)
            {
                return HttpNotFound();
            }
            return View(citas);
        }

        // POST: Citas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            Citas citas = db.Citas.Find(id);
            db.Citas.Remove(citas);
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
