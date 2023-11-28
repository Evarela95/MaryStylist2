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
    public class GenerarFacturaPDF
    {
        public static void GenerateFacturaPDF(int id, string Nombre_Usuario, string Apellido_Usuario)
        {

            //RUTA DE CONEXION
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

            //PROCEDIMIENTO ALMACENADO QUE RECIBE UN ID
            string query = "EXEC sp_DescargarFactura @Id_Factura";

            // RUTA DONDE SE GUARDA EL PDF
            string rutaGuardado = "C:\\Users\\esteb\\source\\repos\\MaryStylist2\\WebApplication1\\Facturas\\";

            //OBTENER FECHA
            string fechaHoraActual = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            //ASIGNAR NOMBRE AL DOCUMENTO PDF NUEVO
            string nombrePDF = $"{Nombre_Usuario + " " + Apellido_Usuario}" + fechaHoraActual + ".pdf"; // Nombre del archivo con el nombre del usuario

            // CREA EL DOC
            Document doc = new Document();

            // CREA OBJETO FILE STREAM
            using (FileStream fs = new FileStream(Path.Combine(rutaGuardado, nombrePDF), FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id_Factura", id);
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
                                Paragraph title = new Paragraph("Factura Electrónica", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24, BaseColor.BLACK));
                                title.Alignment = Element.ALIGN_CENTER;

                                // Añadir el título al documento PDF
                                doc.Add(title);

                                // Agregar dos espacios debajo del título
                                doc.Add(Chunk.NEWLINE);
                                doc.Add(Chunk.NEWLINE);

                                // Crear una tabla con tantas filas como celdas tenga la original y 2 columnas
                                PdfPTable table = new PdfPTable(2);
                                table.WidthPercentage = 80;

                                // Agregar celdas a la tabla invertida
                                AddCellWithBorders(table, "ID", row["Id_Factura"].ToString());
                                AddCellWithBorders(table, "Cliente", row["Nombre_Usuario"].ToString() + " " + row["Apellido_Usuario"].ToString());
                                AddCellWithBorders(table, "Fecha", row["Fecha"].ToString());
                                AddCellWithBorders(table, "Nombre del servicio", row["Nombre"].ToString());
                                AddCellWithBorders(table, "Descripción", row["Descripcion"].ToString());
                                AddCellWithBorders(table, "Nombre empleado", row["Nombre_Empleado"].ToString());
                                AddCellWithBorders(table, "Apellido empleado", row["Descripcion"].ToString());
                                AddCellWithBorders(table, "Total", row["Total"].ToString());

                                // Añadir la tabla al documento PDF
                                doc.Add(table);
                            }
                        }
                    }
                }

                doc.Close();
            }
        }

        // Función para agregar celda a la tabla con bordes
        private static void AddCellWithBorders(PdfPTable table, string header, string content)
        {
            PdfPCell cell = new PdfPCell(new Phrase(header));
            cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase(content));
            cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            table.AddCell(cell);
        }
    }
}