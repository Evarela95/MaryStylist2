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
    public class PromocionesServiciosController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();

        // GET: PromocionesServicios
        public ActionResult Index()
        {
            var promocionesServicios = db.PromocionesServicios.Include(p => p.Servicios);
            return View(promocionesServicios.ToList());
        }

        // GET: PromocionesServicios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromocionesServicios promocionesServicios = db.PromocionesServicios.Find(id);
            if (promocionesServicios == null)
            {
                return HttpNotFound();
            }
            return View(promocionesServicios);
        }

        // GET: PromocionesServicios/Create
        public ActionResult Create()
        {
            ViewBag.Id_Servicio = new SelectList(db.Servicios, "Id_Servicio", "Nombre_Servicio");
            return View();
        }

        // POST: PromocionesServicios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Promo_Servicio,Nombre_Promo,Descripcion_Promo,Precio_Promo,Fecha_Inicio,Fecha_Final,Id_Servicio")] PromocionesServicios promocionesServicios)
        {
            if (ModelState.IsValid)
            {
                db.PromocionesServicios.Add(promocionesServicios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Servicio = new SelectList(db.Servicios, "Id_Servicio", "Nombre_Servicio", promocionesServicios.Id_Servicio);
            return View(promocionesServicios);
        }

        // GET: PromocionesServicios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromocionesServicios promocionesServicios = db.PromocionesServicios.Find(id);
            if (promocionesServicios == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Servicio = new SelectList(db.Servicios, "Id_Servicio", "Nombre_Servicio", promocionesServicios.Id_Servicio);
            return View(promocionesServicios);
        }

        // POST: PromocionesServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Promo_Servicio,Nombre_Promo,Descripcion_Promo,Precio_Promo,Fecha_Inicio,Fecha_Final,Id_Servicio")] PromocionesServicios promocionesServicios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promocionesServicios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Servicio = new SelectList(db.Servicios, "Id_Servicio", "Nombre_Servicio", promocionesServicios.Id_Servicio);
            return View(promocionesServicios);
        }

        // GET: PromocionesServicios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromocionesServicios promocionesServicios = db.PromocionesServicios.Find(id);
            if (promocionesServicios == null)
            {
                return HttpNotFound();
            }
            return View(promocionesServicios);
        }

        // POST: PromocionesServicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PromocionesServicios promocionesServicios = db.PromocionesServicios.Find(id);
            db.PromocionesServicios.Remove(promocionesServicios);
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

        public ActionResult PromoServImagen(int id)
        {
            PromocionesServicios promocionesserv = db.PromocionesServicios.Include(p => p.ImagenesPromosServ).FirstOrDefault(p => p.Id_Promo_Servicio == id);
            if (promocionesserv == null)
            {
                return HttpNotFound();
            }

            return View(promocionesserv);
        }

        // Acción para mostrar el formulario de creación de una nueva imagen para el producto
        public ActionResult AddImage(int id)
        {
            PromocionesServicios promocionesserv = db.PromocionesServicios.Find(id);
            if (promocionesserv == null)
            {
                return HttpNotFound();
            }

            return View(new ImagenesPromosServ { Id_Promo_Servicio = id });
        }

        // Acción para guardar la nueva imagen en la base de datos
        [HttpPost]
        public ActionResult AddImage(ImagenesPromosServ image, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                using (var reader = new BinaryReader(file.InputStream))
                {
                    image.ImageData = reader.ReadBytes(file.ContentLength);
                }

                // Establece la relación entre la imagen y el producto
                image.PromocionesServicios = db.PromocionesServicios.Find(image.Id_Promo_Servicio);

                db.ImagenesPromosServ.Add(image);
                db.SaveChanges();
                return RedirectToAction("PromoServImagen", new { id = image.Id_Promo_Servicio });
            }

            return View(image);
        }

        // Acción para mostrar el formulario de edición de una imagen
        public ActionResult EditImage(int id)
        {
            ImagenesPromosServ image = db.ImagenesPromosServ.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        // Acción para actualizar los datos de una imagen en la base de datos
        [HttpPost]
        public ActionResult EditImage(ImagenesPromosServ image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ImagenesPromosServ", new { id = image.Id_Promo_Servicio });
            }

            return View(image);
        }

        // Acción para eliminar una imagen de la base de datos
        public ActionResult DeleteImage(int id)
        {
            ImagenesPromosServ image = db.ImagenesPromosServ.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("DeleteImage")]
        public ActionResult ConfirmarEliminarImagen(int id)
        {
            ImagenesPromosServ image = db.ImagenesPromosServ.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            db.ImagenesPromosServ.Remove(image);
            db.SaveChanges();
            return RedirectToAction("PromoServImagen", new { id = image.Id_Promo_Servicio });
        }
    }
}
