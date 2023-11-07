using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PlanillaEmpleados
    {
        public static void GeneratePlanillaPDF(int idEmpleado, string Nombre_Empleado, string Apellido_Empleado)
        {
            // Tu cadena de conexión y consulta SQL

            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            string query = "EXEC sp_ConsultaPlanillaConFiltro @Id";


            
           

            // Especifica la ruta donde se guardará el archivo PDF
            string rutaGuardado = "C:\\Users\\esteb\\source\\repos\\MaryStylist2\\WebApplication1\\Planilla\\";
            string fechaHoraActual = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string nombreArchivoPDF = $"{Nombre_Empleado + " " + Apellido_Empleado}" + fechaHoraActual + ".pdf"; // Nombre del archivo con el nombre del empleado


            // Creamos un documento PDF
            Document doc = new Document();

            // Utiliza la ruta completa al crear el objeto FileStream
            using (FileStream fs = new FileStream(Path.Combine(rutaGuardado, nombreArchivoPDF), FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", idEmpleado);
                        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            dataAdapter.Fill(dataTable);

                            if (dataTable.Rows.Count > 0)
                            {
                                // Agregamos los datos al documento PDF
                                DataRow row = dataTable.Rows[0];

                                doc.Add(new Paragraph("Boleta de pago"));
                                doc.Add(new Paragraph("Nombre: " + row["Nombre_Empleado"].ToString() + " " + row["Apellido_Empleado"].ToString()));
                                doc.Add(new Paragraph("Salario Bruto: " + row["Salario"].ToString()));
                                doc.Add(new Paragraph("CCSS (10.67%): " + row["CCSS"].ToString()));
                                doc.Add(new Paragraph("Banco Popular: " + row["BP"].ToString()));

                                // Puedes seguir agregando más información aquí según tus necesidades
                            }
                        }
                    }
                }

                doc.Close();
            }
        }
    }
}