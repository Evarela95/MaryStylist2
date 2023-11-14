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
    public class ProductosAdminController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();

        // GET: ProductosAdmin
        public ActionResult Index()
        {
            return View(db.Productos.ToList());
        }

        // GET: ProductosAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // GET: ProductosAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductosAdmin/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Producto,Nombre_Producto,Descripcion_Producto,Precio_Producto,Stock")] Productos productos)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(productos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productos);
        }

        // GET: ProductosAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: ProductosAdmin/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Producto,Nombre_Producto,Descripcion_Producto,Precio_Producto,Stock")] Productos productos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productos);
        }

        // GET: ProductosAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: ProductosAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos productos = db.Productos.Find(id);
            db.Productos.Remove(productos);
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
        public ActionResult ProductoImagen(int id)
        {
            Productos productos = db.Productos.Include(p => p.ImagenesProductos).FirstOrDefault(p => p.Id_Producto == id);
            if (productos == null)
            {
                return HttpNotFound();
            }

            return View(productos);
        }

        // Acción para mostrar el formulario de creación de una nueva imagen para el producto
        public ActionResult AddImage(int id)
        {
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }

            return View(new ImagenesProductos { Id_Producto = id });
        }

        // Acción para guardar la nueva imagen en la base de datos
        [HttpPost]
        public ActionResult AddImage(ImagenesProductos imagenes, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                using (var reader = new BinaryReader(file.InputStream))
                {
                    imagenes.Foto_Producto = reader.ReadBytes(file.ContentLength);
                }

                // Establece la relación entre la imagen y el producto
                imagenes.Productos = db.Productos.Find(imagenes.Id_Producto);

                db.ImagenesProductos.Add(imagenes);
                db.SaveChanges();
                return RedirectToAction("ProductoImagen", new { id = imagenes.Id_Producto });
            }

            return View(imagenes);
        }

        // Acción para mostrar el formulario de edición de una imagen
        public ActionResult EditImage(int id)
        {
            ImagenesProductos imagenes = db.ImagenesProductos.Find(id);
            if (imagenes == null)
            {
                return HttpNotFound();
            }

            return View(imagenes);
        }

        // Acción para actualizar los datos de una imagen en la base de datos
        [HttpPost]
        public ActionResult EditImage(ImagenesProductos imagenes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(imagenes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductoImagen", new { id = imagenes.Id_Producto });
            }

            return View(imagenes);
        }

        // Acción para eliminar una imagen de la base de datos
        public ActionResult DeleteImage(int id)
        {
            ImagenesProductos imagenes = db.ImagenesProductos.Find(id);
            if (imagenes == null)
            {
                return HttpNotFound();
            }

            return View(imagenes);
        }

        [HttpPost, ActionName("DeleteImage")]
        public ActionResult ConfirmarEliminarImagen(int id)
        {
            ImagenesProductos imagenes = db.ImagenesProductos.Find(id);
            if (imagenes == null)
            {
                return HttpNotFound();
            }

            db.ImagenesProductos.Remove(imagenes);
            db.SaveChanges();
            return RedirectToAction("ProductoImagen", new { id = imagenes.Id_Producto });
        }
    }
}
    

