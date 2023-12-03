using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
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
            string rutaGuardado = "C:\\Users\\melme\\source\\repos\\MaryStylist2\\WebApplication1\\Planilla\\";

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
                                DataRow row = dataTable.Rows[0];

                                // Crear un párrafo para el título
                                Paragraph title = new Paragraph("Mari Stylist", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 24, BaseColor.BLACK));
                                title.Alignment = Element.ALIGN_LEFT;
                                doc.Add(title);

                                // Crear un párrafo para el subtítulo
                                Paragraph subTitle = new Paragraph("Planilla de Empleado", FontFactory.GetFont(FontFactory.HELVETICA, 18, BaseColor.BLACK));
                                subTitle.Alignment = Element.ALIGN_CENTER;
                                doc.Add(subTitle);

                                // Añadir una línea divisoria
                                doc.Add(new Chunk(new LineSeparator(0.5f, 100, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));

                                // Agregar dos espacios debajo del subtítulo
                                doc.Add(Chunk.NEWLINE);

                                // Crear una tabla para los datos del empleado
                                PdfPTable table = new PdfPTable(2);
                                table.WidthPercentage = 60;
                                table.HorizontalAlignment = Element.ALIGN_CENTER;

                                // Agregar celdas a la tabla
                                AddCellWithBorders(table, "Nombre", row["Nombre_Empleado"].ToString() + " " + row["Apellido_Empleado"].ToString());
                                AddCellWithBorders(table, "Puesto", row["Descripcion_Empleado"].ToString());
                                AddCellWithBorders(table, "Correo", row["Correo_Empleado"].ToString());
                                AddCellWithBorders(table, "Salario Bruto", row["Salario"].ToString());
                                AddCellWithBorders(table, "CCSS (10.67%)", row["CCSS"].ToString());
                                AddCellWithBorders(table, "Banco Popular", row["BP"].ToString());
                                AddCellWithBorders(table, "Salario Neto", row["SalarioNeto"].ToString());

                                // Añadir la tabla al documento PDF
                                doc.Add(table);
                            }
                        }
                    }
                }

                doc.Close();
            }
        }

        private static void AddCellWithBorders(PdfPTable table, string header, string value)
        {
            PdfPCell cellHeader = new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK)));
            PdfPCell cellValue = new PdfPCell(new Phrase(value, FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK)));

            // Establecer el estilo de las celdas
            cellHeader.BackgroundColor = BaseColor.LIGHT_GRAY;
            cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellHeader.Padding = 8;

            cellValue.HorizontalAlignment = Element.ALIGN_LEFT;
            cellValue.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellValue.Padding = 8;

            // Agregar celdas a la tabla
            table.AddCell(cellHeader);
            table.AddCell(cellValue);
        }
    }


}