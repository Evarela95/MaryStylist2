using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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

        // GET: Citas
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            var citas = db.Citas
                               .Where(c => c.Estado && c.Fecha_Cita == DateTime.Today)
                               .Include(c => c.AspNetUsers)
                               .Include(c => c.Empleados)
                               .Include(c => c.Servicios_Productos)
                               
                               .ToList();


            return View(citas.ToList());
        }

        
        [Authorize(Roles ="Administrador")]
             public ActionResult TodasLasCitas()
        {
            var citas = db.Citas
                               .Where(c => c.Estado)
                               .Include(c => c.AspNetUsers)
                               .Include(c => c.Empleados)
                               .Include(c => c.Servicios_Productos)

                               .ToList();


            return View(citas.ToList());
        }


        [Authorize(Roles = "Administrador")]
        public ActionResult CitasFinalizadas()
        {
            var citas = db.Citas
                               .Where(c => !c.Estado)
                               .Include(c => c.AspNetUsers)
                               .Include(c => c.Empleados)
                               .Include(c => c.Servicios_Productos)
                               .ToList();


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




        public ActionResult CitaExitosa()
        {

            return View();
        }


        // -------------------------------------CITAS QUE NO TIENEN PROMOCION -----------------------------
        [Authorize]
        public ActionResult CitasNoPromo()
        {
            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado");

            var serviciosDeCategoria1 = db.Servicios_Productos.Where(sp => sp.Id_Categoria == 1 && !sp.Promo).ToList();

        
            var serviciosConPrecio = serviciosDeCategoria1.Select(sp => new SelectListItem
            {
                Value = sp.Id.ToString(), 
                Text = $"{sp.Nombre} - ₡{sp.Precio}" 
            }).ToList();

            ViewBag.Id_Serv_Prod = new SelectList(serviciosConPrecio, "Value", "Text");

            return View();
        }





        // POST: Citas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CitasNoPromo([Bind(Include = "Id_Cita,Fecha_Cita,Hora_Cita,Id_Usuario,Id_Empleado,Id_Serv_Prod,Estado")] Citas citas)
        {
            if (ModelState.IsValid)
            {
                var estado = true;
                citas.Estado = estado;
                string userId = User.Identity.GetUserId();

                citas.Id_Usuario = userId;

                db.Citas.Add(citas);
                db.SaveChanges();
                return RedirectToAction("CitaExitosa");
            }


            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado", citas.Id_Empleado);
            ViewBag.Id_Serv_Prod = new SelectList(db.Servicios_Productos, "Id", "Nombre", citas.Id_Serv_Prod);
            return View(citas);
        }

        //------------------------------------------------------------------------------------------------


        //CITAS CON PROMO----------------------------------------------------------

        // GET: Citas/Create
        [Authorize]
        public ActionResult CitasPromo()
        {

            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado");

            var serviciosDeCategoria1 = db.Servicios_Productos.Where(sp => sp.Id_Categoria == 1 && sp.Promo).ToList();

            var serviciosConPrecio = serviciosDeCategoria1.Select(sp => new SelectListItem
            {
                Value = sp.Id.ToString(), 
                Text = $"{sp.Nombre} - ₡{sp.Precio_Promo}" 
            }).ToList();

           
            ViewBag.Id_Serv_Prod = new SelectList(serviciosConPrecio, "Value", "Text");

            return View();
        }





        // POST: Citas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CitasPromo([Bind(Include = "Id_Cita,Fecha_Cita,Hora_Cita,Id_Usuario,Id_Empleado,Id_Serv_Prod,Estado")] Citas citas)
        {
            if (ModelState.IsValid)
            {
                var estado = true;
                citas.Estado = estado;
                string userId = User.Identity.GetUserId();

                citas.Id_Usuario = userId;

                db.Citas.Add(citas);
                db.SaveChanges();
                return RedirectToAction("CitaExitosa");
            }


            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado", citas.Id_Empleado);
            ViewBag.Id_Serv_Prod = new SelectList(db.Servicios_Productos, "Id", "Nombre", citas.Id_Serv_Prod);
            return View(citas);
        }

        //---------------------------------------------------------------------------------------------







        // GET: Citas/Edit/5
        [Authorize(Roles = "Administrador")]
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
            ViewBag.Id_Serv_Prod = new SelectList(db.Servicios_Productos, "Id", "Nombre", citas.Id_Serv_Prod);
            return View(citas);
        }

        // POST: Citas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Cita,Fecha_Cita,Hora_Cita,Id_Usuario,Id_Empleado,Id_Serv_Prod,Estado")] Citas citas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(citas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Usuario = new SelectList(db.AspNetUsers, "Id", "Email", citas.Id_Usuario);
            ViewBag.Id_Empleado = new SelectList(db.Empleados, "Id_Empleado", "Nombre_Empleado", citas.Id_Empleado);
            ViewBag.Id_Serv_Prod = new SelectList(db.Servicios_Productos, "Id", "Nombre", citas.Id_Serv_Prod);
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



        ///----------------------------------------------------------------------------------------------------

        [Authorize]
        public ActionResult MisCitas(string idUsuario)
        {
            DataTable citasPorUsuario = ObtenerCitasPorUsuario(idUsuario);

            // Pasar los datos a la vista
            return View(citasPorUsuario);
        }


       
        protected DataTable ObtenerCitasPorUsuario(string idUsuario)
        {
            string userId = User.Identity.GetUserId();

            idUsuario = userId;

            DataTable dataTable = new DataTable();
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerCitasPorUsuario", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    dataTable.Load(reader);
                }
            }

            return dataTable;
        }

        ///----------------------------------------------------------------------------------------------------

        ///----------------------------------------------------------------------------------------------------



        ///----------------------------------------------------------------------------------------------------

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






