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
    [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        public ActionResult FinalizarCita(int id)
        {
            // Obtener la cita por su ID
            Citas cita = db.Citas.Find(id);

            // Verificar si la cita existe
            if (cita == null)
            {
                return HttpNotFound();
            }

            // Verificar si la cita ya está finalizada
            if (!cita.Estado)
            {
                // Si ya está finalizada, redirigir a la acción Index
                return RedirectToAction("Index");
            }

            // Marcar la cita como finalizada
            cita.Estado = false;

            // Guardar los cambios en la base de datos
            db.SaveChanges();

            // Crear una instancia de GananciaDiaria
            var gananciaDiaria = new GananciaDiaria
            {
                // Puedes asignar valores específicos o calcularlos según tus necesidades
                Ingresos = cita.Servicios_Productos.Precio_Promo ?? cita.Servicios_Productos.Precio,
                Fecha = DateTime.Today,
                Egresos = 0 // Otra lógica de cálculo de egresos, si es necesario
            };

            // Agregar la instancia de GananciaDiaria a la base de datos
            db.GananciaDiaria.Add(gananciaDiaria);

            // Guardar los cambios en la base de datos
            db.SaveChanges();

            // Redirigir a la acción Index de GananciaDiaria
            return RedirectToAction("Index", "GananciaDiarias1");
        }

    }
}
