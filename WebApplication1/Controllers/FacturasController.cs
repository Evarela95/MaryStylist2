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
    public class FacturasController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();

        // GET: Facturas
        public ActionResult Index()
        {
            var facturas = db.Facturas.Include(f => f.AspNetUsers).Include(f => f.Citas);
            return View(facturas.ToList());
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        //----------------------------------------------------------------------------------------
        [Authorize]
        public ActionResult MisFacturas(string idUsuario)
        {
            DataTable citasPorUsuario = ObtenerFacturasPorUsuario(idUsuario);

            // Pasar los datos a la vista
            return View(citasPorUsuario);
        }



        protected DataTable ObtenerFacturasPorUsuario(string idUsuario)
        {
            string userId = User.Identity.GetUserId();

            idUsuario = userId;

            DataTable dataTable = new DataTable();
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_FacturasPorUsuario", connection))
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



        //----------------------------------------------------------------------------------------

        public ActionResult ImprimirFacturaPDF(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facturas facturas = db.Facturas.Find(id);
            GenerarFacturaPDF.GenerateFacturaPDF(id);
            if (facturas == null)
            {
                return HttpNotFound();
            }
            return View(facturas);
        }

    }
}
