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
    public class PromocionesProductosController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();

        // GET: PromocionesProductos
        public ActionResult Index()
        {
            var promocionesProductos = db.PromocionesProductos.Include(p => p.Productos);
            return View(promocionesProductos.ToList());
        }

        // GET: PromocionesProductos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromocionesProductos promocionesProductos = db.PromocionesProductos.Find(id);
            if (promocionesProductos == null)
            {
                return HttpNotFound();
            }
            return View(promocionesProductos);
        }

        // GET: PromocionesProductos/Create
        public ActionResult Create()
        {
            ViewBag.Id_Producto = new SelectList(db.Productos, "Id_Producto", "Nombre_Producto");
            return View();
        }

        // POST: PromocionesProductos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Promo_Producto,Nombre_Promo_Producto,Descripcion_Promo_Producto,Precio_Nuevo_Producto,Fecha_Inicio_Promo,Fecha_Final_Promo,Id_Producto")] PromocionesProductos promocionesProductos)
        {
            if (ModelState.IsValid)
            {
                db.PromocionesProductos.Add(promocionesProductos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Producto = new SelectList(db.Productos, "Id_Producto", "Nombre_Producto", promocionesProductos.Id_Producto);
            return View(promocionesProductos);
        }

        // GET: PromocionesProductos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromocionesProductos promocionesProductos = db.PromocionesProductos.Find(id);
            if (promocionesProductos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Producto = new SelectList(db.Productos, "Id_Producto", "Nombre_Producto", promocionesProductos.Id_Producto);
            return View(promocionesProductos);
        }

        // POST: PromocionesProductos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Promo_Producto,Nombre_Promo_Producto,Descripcion_Promo_Producto,Precio_Nuevo_Producto,Fecha_Inicio_Promo,Fecha_Final_Promo,Id_Producto")] PromocionesProductos promocionesProductos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(promocionesProductos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Producto = new SelectList(db.Productos, "Id_Producto", "Nombre_Producto", promocionesProductos.Id_Producto);
            return View(promocionesProductos);
        }

        // GET: PromocionesProductos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromocionesProductos promocionesProductos = db.PromocionesProductos.Find(id);
            if (promocionesProductos == null)
            {
                return HttpNotFound();
            }
            return View(promocionesProductos);
        }

        // POST: PromocionesProductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PromocionesProductos promocionesProductos = db.PromocionesProductos.Find(id);
            db.PromocionesProductos.Remove(promocionesProductos);
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

        public ActionResult PromoProdImagen(int id)
        {
            PromocionesProductos promocionesprod = db.PromocionesProductos.Include(p => p.ImagenesPromosProd).FirstOrDefault(p => p.Id_Promo_Producto == id);
            if (promocionesprod == null)
            {
                return HttpNotFound();
            }

            return View(promocionesprod);
        }

        // Acción para mostrar el formulario de creación de una nueva imagen para el producto
        public ActionResult AddImage(int id)
        {
            PromocionesProductos promocionesprod = db.PromocionesProductos.Find(id);
            if (promocionesprod == null)
            {
                return HttpNotFound();
            }

            return View(new ImagenesPromosProd { Id_Promo_Producto = id });
        }

        // Acción para guardar la nueva imagen en la base de datos
        [HttpPost]
        public ActionResult AddImage(ImagenesPromosProd image, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                using (var reader = new BinaryReader(file.InputStream))
                {
                    image.ImageData = reader.ReadBytes(file.ContentLength);
                }

                // Establece la relación entre la imagen y el producto
                image.PromocionesProductos = db.PromocionesProductos.Find(image.Id_Promo_Producto);

                db.ImagenesPromosProd.Add(image);
                db.SaveChanges();
                return RedirectToAction("PromoProdImagen", new { id = image.Id_Promo_Producto });
            }

            return View(image);
        }
       
        // Acción para mostrar el formulario de edición de una imagen
        public ActionResult EditImage(int id)
        {
            ImagenesPromosProd image = db.ImagenesPromosProd.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        // Acción para actualizar los datos de una imagen en la base de datos
        [HttpPost]
        public ActionResult EditImage(ImagenesPromosProd image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ImagenesPromosProd", new { id = image.Id_Promo_Producto });
            }

            return View(image);
        }

        // Acción para eliminar una imagen de la base de datos
        public ActionResult DeleteImage(int id)
        {
            ImagenesPromosProd image = db.ImagenesPromosProd.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("DeleteImage")]
        public ActionResult ConfirmarEliminarImagen(int id)
        {
            ImagenesPromosProd image = db.ImagenesPromosProd.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }

            db.ImagenesPromosProd.Remove(image);
            db.SaveChanges();
            return RedirectToAction("PromoProdImagen", new { id = image.Id_Promo_Producto });
        }
    }
}
