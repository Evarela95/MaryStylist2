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
            
            //RUTA DE CONEXION
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            //PROCEDIMIENTO ALMACENADO QUE RECIBE UN ID
            string query = "EXEC sp_ConsultaPlanillaConFiltro @Id";


            
           

            // ERUTA DONDE SE GUARDA EL PDF
            string rutaGuardado = "C:\\Users\\esteb\\source\\repos\\MaryStylist2\\WebApplication1\\Planilla\\";

            //OBTENER FECHA
            string fechaHoraActual = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            //ASIGNAR NOMBRE AL DOCUMENTO PDF NUEVO
            string nombreArchivoPDF = $"{Nombre_Empleado + " " + Apellido_Empleado}" + fechaHoraActual + ".pdf"; // Nombre del archivo con el nombre del empleado


            // CREA EL DOC
            Document doc = new Document();

            // CREA OBJETO FILE STREAM
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
                                // Agregamos los datos al documento PDF
                                DataRow row = dataTable.Rows[0];

                                // Crear un párrafo para el título
                                Paragraph title = new Paragraph("Boleta de pago", FontFactory.GetFont(FontFactory.HELVETICA, 18, BaseColor.BLUE));
                                title.Alignment = Element.ALIGN_CENTER;

                                // Añadir el título al documento PDF
                                doc.Add(title);

                                // Agregar dos espacios debajo del título
                                doc.Add(Chunk.NEWLINE);
                                doc.Add(Chunk.NEWLINE);

                                // Crear una tabla con tantas filas como celdas tenga la original y 2 columnas
                                PdfPTable table = new PdfPTable(2);

                                // Agregar celdas a la tabla invertida
                                table.AddCell("Nombre");
                                table.AddCell(row["Nombre_Empleado"].ToString() + " " + row["Apellido_Empleado"].ToString());

                                table.AddCell("Puesto ");
                                table.AddCell(row["Descripcion_Empleado"].ToString());

                                table.AddCell("Correo ");
                                table.AddCell(row["Correo_Empleado"].ToString());



                                table.AddCell("Salario Bruto");
                                table.AddCell(row["Salario"].ToString());

                                table.AddCell("CCSS (10.67%)");
                                table.AddCell(row["CCSS"].ToString());

                                table.AddCell("Banco Popular");
                                table.AddCell(row["BP"].ToString());


                                table.AddCell("Salario neto");
                                table.AddCell(row["SalarioNeto"].ToString());

                                // Añadir la tabla al documento PDF
                                doc.Add(table);
                            }
                        }
                    }
                }

                doc.Close();
            }
        }
    }
}