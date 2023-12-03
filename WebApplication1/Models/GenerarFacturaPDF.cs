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
        public static void GenerateFacturaPDF(int id)
        {

            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            string query = "EXEC sp_DescargarFactura @Id_Factura";
            string rutaGuardado = "C:\\Users\\melme\\source\\repos\\MaryStylist2\\WebApplication1\\Facturas\\";
            string fechaHoraActual = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string nombrePDF =  fechaHoraActual + ".pdf"; 

            Document doc = new Document();
            doc.AddAuthor("Mary Stylist");
            doc.AddTitle("Factura Electrónica");

            using (FileStream fs = new FileStream(Path.Combine(rutaGuardado, nombrePDF), FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Agregar encabezado
                AddHeader(doc);

                //Agregar título
                AddTitle(doc);

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
                                DataRow row = dataTable.Rows[0];

                                // Mostrar ID de factura, Usuario y Fecha sin bordes
                                AddDataWithoutBorders(doc, "ID de Factura", row["Id_Factura"].ToString());
                                AddDataWithoutBorders(doc, "Cliente", row["UserName"].ToString());
                                AddDataWithoutBorders(doc, "Fecha", row["Fecha"].ToString());

                                // Agregar espacio después de la primera tabla
                                doc.Add(Chunk.NEWLINE);

                                // Crear una nueva tabla para el resto de los detalles
                                PdfPTable detailsTable = new PdfPTable(2);
                                detailsTable.WidthPercentage = 100;
                                detailsTable.SpacingBefore = 10;

                                // Establecer el estilo de las celdas para los detalles
                                PdfPCell detailCellHeader = new PdfPCell();
                                PdfPCell detailCellValue = new PdfPCell();

                                detailCellHeader.BackgroundColor = BaseColor.LIGHT_GRAY;
                                detailCellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                detailCellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                                detailCellHeader.Padding = 5;

                                detailCellValue.HorizontalAlignment = Element.ALIGN_LEFT;
                                detailCellValue.VerticalAlignment = Element.ALIGN_MIDDLE;
                                detailCellValue.Padding = 5;
                                detailCellValue.Colspan = 2;

                                // Agregar celdas para el resto de los detalles
                                AddCellWithBorders(detailsTable, detailCellHeader, detailCellValue, "Nombre del Servicio", row["Nombre"].ToString());
                                AddCellWithBorders(detailsTable, detailCellHeader, detailCellValue, "Descripción", row["Descripcion"].ToString());
                                AddCellWithBorders(detailsTable, detailCellHeader, detailCellValue, "Nombre Empleado", row["Nombre_Empleado"].ToString());
                                AddCellWithBorders(detailsTable, detailCellHeader, detailCellValue, "Apellido Empleado", row["Apellido_Empleado"].ToString());
                                AddCellWithBorders(detailsTable, detailCellHeader, detailCellValue, "Total", row["Total"].ToString());

                                doc.Add(detailsTable);
                            }
                        }
                    }
                }
                //Agregar footer
                AddFooter(doc);

                doc.Close();
            }
        }

        private static void AddHeader(Document doc)
        {

            PdfPTable headerTable = new PdfPTable(1);
            headerTable.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("FACTURA ELECTRÓNICA", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = Rectangle.NO_BORDER;

            headerTable.AddCell(cell);

            doc.Add(headerTable);
        }

        private static void AddTitle(Document doc)
        {
            Paragraph title = new Paragraph("Detalles de la Factura", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK));
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 10;

            doc.Add(title);
        }

        private static void AddDataWithoutBorders(Document doc, string header, string value)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Add(new Chunk($"{header}: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK)));
            paragraph.Add(new Chunk(value, FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK)));
            doc.Add(paragraph);
        }

        private static void AddFooter(Document doc)
        {
            // Agregar información adicional y agradecimiento
            Paragraph footer = new Paragraph("Gracias por su preferencia. Para consultas, contacte a nuestro servicio al cliente.", FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK));
            footer.Alignment = Element.ALIGN_CENTER;
            footer.SpacingBefore = 10;

            doc.Add(footer);
        }

        private static void AddCellWithBorders(PdfPTable table, PdfPCell cellHeader, PdfPCell cellValue, string header, string value)
        {
            cellHeader.Phrase = new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK));
            cellValue.Phrase = new Phrase(value, FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK));

            table.AddCell(cellHeader);
            table.AddCell(cellValue);
        }
    }
}