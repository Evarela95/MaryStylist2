﻿using Microsoft.AspNet.Identity;
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
    public class Servicios_ProductosController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();

        // GET: Servicios_Productos
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            var servicios_Productos = db.Servicios_Productos.Include(s => s.Categorias);
            return View(servicios_Productos.ToList());
        }



        public ActionResult Servicios()
        {
           

            var serviciosSinPromo = db.Servicios_Productos
                                        .Include(s => s.Categorias)
                                        .Where(s => s.Categorias.Id_Categoria == 1 && !s.Promo)
                                        .ToList();
            return View(serviciosSinPromo);
        }



        public ActionResult Productos()
        {
            string userId = User.Identity.GetUserId();

            ViewBag.UserId = userId;

            var Categoria2 = db.Servicios_Productos
                                        .Include(s => s.Categorias)
                                        .Where(s => s.Categorias.Id_Categoria == 2 && !s.Promo)
                                        .ToList();
            return View(Categoria2);
        }


        public ActionResult ServiciosPromo()
        {
            var productosCategoria1ConPromo = db.Servicios_Productos
                                                .Include(s => s.Categorias)
                                                .Where(s => s.Categorias.Id_Categoria == 1 && s.Promo)
                                                .ToList();
            return View(productosCategoria1ConPromo);
        }



        public ActionResult ProductosPromo()
        {
            string userId = User.Identity.GetUserId();

            ViewBag.UserId = userId;


            var productosCategoria1ConPromo = db.Servicios_Productos
                                                .Include(s => s.Categorias)
                                                .Where(s => s.Categorias.Id_Categoria == 2 && s.Promo)
                                                .ToList();
            return View(productosCategoria1ConPromo);
        }






        // GET: Servicios_Productos/Details/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicios_Productos servicios_Productos = db.Servicios_Productos.Find(id);
            if (servicios_Productos == null)
            {
                return HttpNotFound();
            }
            return View(servicios_Productos);
        }

        // GET: Servicios_Productos/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Id_Categoria = new SelectList(db.Categorias, "Id_Categoria", "Nombre_Categoria");
            return View();
        }

        // POST: Servicios_Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Precio,Promo,Precio_Promo,Descripcion,Id_Categoria,FileName,ImageData")] Servicios_Productos servicios_Productos, HttpPostedFileBase imagenFile)
        {
            if (ModelState.IsValid)
            {
                if (imagenFile != null && imagenFile.ContentLength > 0)
                {
                    // Guardar la imagen en la carpeta deseada
                    var imagePath = Path.Combine(Server.MapPath("~/imagenes/ImagenesProductosServicios/"), Path.GetFileName(imagenFile.FileName));
                    imagenFile.SaveAs(imagePath);

                    // Asignar el nombre de la imagen al modelo
                    servicios_Productos.FileName = imagenFile.FileName;
                }

                db.Servicios_Productos.Add(servicios_Productos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Categoria = new SelectList(db.Categorias, "Id_Categoria", "Nombre_Categoria", servicios_Productos.Id_Categoria);
            return View(servicios_Productos);
        }

        // GET: Servicios_Productos/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicios_Productos servicios_Productos = db.Servicios_Productos.Find(id);
            if (servicios_Productos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Categoria = new SelectList(db.Categorias, "Id_Categoria", "Nombre_Categoria", servicios_Productos.Id_Categoria);
            return View(servicios_Productos);
        }

        // POST: Servicios_Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Precio,Promo,Precio_Promo,Descripcion,Id_Categoria,FileName,ImageData")] Servicios_Productos servicios_Productos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(servicios_Productos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Categoria = new SelectList(db.Categorias, "Id_Categoria", "Nombre_Categoria", servicios_Productos.Id_Categoria);
            return View(servicios_Productos);
        }

        // GET: Servicios_Productos/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servicios_Productos servicios_Productos = db.Servicios_Productos.Find(id);
            if (servicios_Productos == null)
            {
                return HttpNotFound();
            }
            return View(servicios_Productos);
        }

        // POST: Servicios_Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Servicios_Productos servicios_Productos = db.Servicios_Productos.Find(id);
            db.Servicios_Productos.Remove(servicios_Productos);
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
