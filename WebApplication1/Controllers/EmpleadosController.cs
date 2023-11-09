using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EmpleadosController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();

        // GET: Empleados
        public ActionResult Index()
        {
            return View(db.Empleados.ToList());
        }

        // GET: Empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Empleado,Nombre_Empleado,Apellido_Empleado,Descripcion_Empleado,Correo_Empleado")] Empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.Empleados.Add(empleados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empleados);
        }

        // GET: Empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // POST: Empleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Empleado,Nombre_Empleado,Apellido_Empleado,Descripcion_Empleado,Correo_Empleado")] Empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empleados);
        }

        // GET: Empleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleados empleados = db.Empleados.Find(id);
            db.Empleados.Remove(empleados);
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

        public ActionResult EmpleadoImagen(int id)
        {
            Empleados empleados = db.Empleados.Include(p => p.ImagenesEmpleados).FirstOrDefault(p => p.Id_Empleado == id);
            if (empleados == null)
            {
                return HttpNotFound();
            }

            return View(empleados);
        }

        // Acción para mostrar el formulario de creación de una nueva imagen para el producto
        public ActionResult AddImage(int id)
        {
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }

            return View(new ImagenesEmpleados { Id_Empleado = id });
        }

        // Acción para guardar la nueva imagen en la base de datos
        [HttpPost]
        public ActionResult AddImage(ImagenesEmpleados image, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                using (var reader = new BinaryReader(file.InputStream))
                {
                    image.ImageData = reader.ReadBytes(file.ContentLength);
                }

                // Establece la relación entre la imagen y el producto
                image.Empleados = db.Empleados.Find(image.Id_Empleado);

                db.ImagenesEmpleados.Add(image);
                db.SaveChanges();
                return RedirectToAction("EmpleadoImagen", new { id = image.Id_Empleado });
            }

            return View(image);
        }

        // Acción para mostrar el formulario de edición de una imagen
        public ActionResult EditImage(int id)
        {
            ImagenesEmpleados image = db.ImagenesEmpleados.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        // Acción para actualizar los datos de una imagen en la base de datos
        [HttpPost]
        public ActionResult EditImage(ImagenesEmpleados image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductImages", new { id = image.Id_Empleado });
            }

            return View(image);
        }

        // Acción para eliminar una imagen de la base de datos
        public ActionResult DeleteImage(int id)
        {
            ImagenesEmpleados image = db.ImagenesEmpleados.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("DeleteImage")]
        public ActionResult ConfirmarEliminarImagen(int id)
        {
            ImagenesEmpleados image = db.ImagenesEmpleados.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            db.ImagenesEmpleados.Remove(image);
            db.SaveChanges();
            return RedirectToAction("EmpleadoImagen", new { id = image.Id_Empleado });
        }



        public ActionResult Planilla(int id, string nombre, string apellido)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            PlanillaEmpleados.GeneratePlanillaPDF(id, nombre, apellido);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }
    }
}
