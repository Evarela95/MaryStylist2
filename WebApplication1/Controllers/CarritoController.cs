using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CarritoController : Controller
    {
        private BD_MARYSTYLISEntities db = new BD_MARYSTYLISEntities();
        // GET: Carrito


        [HttpPost]
        public ActionResult AgregarProductoAlCarrito(int idProducto, string idUsuario)
        {
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_Agregar_producto_carrito", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    command.Parameters.AddWithValue("@Id_Producto", idProducto);
                    command.Parameters.AddWithValue("@Id_usuario", idUsuario);
                

                    try
                    {
                        command.ExecuteNonQuery();
                        return Json(new { success = true, message = "Producto agregado al carrito correctamente." });
                    }
                    catch (SqlException ex)
                    {
                        // Manejar errores de base de datos si es necesario
                        return Json(new { success = false, message = "Error al agregar el producto al carrito." });
                    }
                }
            }
        }


        public ActionResult MiCarrito(string idUsuario)
        {
            string userId = User.Identity.GetUserId();

            ViewBag.UserId = userId;

            DataTable carrito = ObtenerProductosCarrito(idUsuario);

            // Pasar los datos a la vista
            return View(carrito);
        }



        protected DataTable ObtenerProductosCarrito(string idUsuario)
        {
            string userId = User.Identity.GetUserId();

            idUsuario = userId;

            DataTable dataTable = new DataTable();
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_MiCarrito", connection))
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





        [HttpPost]
        public ActionResult AumentarCantidad(int idProducto, string idUsuario)
        {
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_AumentarCantidad", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    command.Parameters.AddWithValue("@IdProducto", idProducto);
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);


                    try
                    {
                        command.ExecuteNonQuery();
                        return Json(new { success = true, message = "Aumentado" });
                    }
                    catch (SqlException ex)
                    {
                        // Manejar errores de base de datos si es necesario
                        return Json(new { success = false, message = "Error." });
                    }
                }
            }
        }




        

             [HttpPost]
        public ActionResult DisminuirCantidad(int idProducto, string idUsuario)
        {
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_DisminuirCantidad", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    command.Parameters.AddWithValue("@IdProducto", idProducto);
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);


                    try
                    {
                        command.ExecuteNonQuery();
                        return Json(new { success = true, message = "Disminuído" });
                    }
                    catch (SqlException ex)
                    {
                        // Manejar errores de base de datos si es necesario
                        return Json(new { success = false, message = "Error." });
                    }
                }
            }
        }




        public decimal ObtenerTotalAPagar(string idUsuario)
        {
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_ObtenerTotalAPagar", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    command.Parameters.Add("@TotalAPagar", SqlDbType.Decimal).Direction = ParameterDirection.Output;

                    try
                    {
                        command.ExecuteNonQuery();

                        if (command.Parameters["@TotalAPagar"].Value != DBNull.Value)
                        {
                            decimal totalAPagar = Convert.ToDecimal(command.Parameters["@TotalAPagar"].Value);
                            return totalAPagar;
                        }
                        else
                        {
                            // Manejar el caso en que el valor retornado sea DBNull
                            return 0; // O cualquier valor predeterminado que desees
                        }
                    }
                    catch (SqlException ex)
                    {
                        // Manejar errores de base de datos si es necesario
                        return 0; // O cualquier valor predeterminado que desees
                    }
                }
            }
        }



        [HttpPost]
        public ActionResult EliminarProducto(int idProducto, string idUsuario)
        {
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_EliminarProducto", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    command.Parameters.AddWithValue("@IdProducto", idProducto);
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);


                    try
                    {
                        command.ExecuteNonQuery();
                        return Json(new { success = true, message = "Eliminado" });
                    }
                    catch (SqlException ex)
                    {
                        // Manejar errores de base de datos si es necesario
                        return Json(new { success = false, message = "Error." });
                    }
                }
            }
        }




        [HttpPost]
        public ActionResult PagarCarrito(decimal totalAPagar, string idUsuario)
        {
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SP_PagarCarrito ", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros al procedimiento almacenado
                    command.Parameters.AddWithValue("@Total", totalAPagar);
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);


                    try
                    {
                        command.ExecuteNonQuery();
                        return View("PaginaPago");
                    }
                    catch (SqlException ex)
                    {
                        // Manejar errores de base de datos si es necesario
                        return Json(new { success = false, message = "Error." });
                    }
                }
            }
        }

        public ActionResult PaginaPago( )
        {
            

            return View();
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