﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace WebApplication1.Models
{
    public class GenerateFacturasProductosPDF
    {
        public static void GenerarFacturaProductosPDF(int id)
        {
            string connectionString = "Server=localhost\\sqlexpress;Database=BD_MARYSTYLIS;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            string query = "EXEC IMPRIMIR_FACTURA_PRODUCTOS @Id_Factura";
            string rutaGuardado = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\";
            string fechaHoraActual = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string nombrePDF = fechaHoraActual + ".pdf";

            Document doc = new Document();
            doc.AddTitle("Factura Electrónica");

            using (FileStream fs = new FileStream(Path.Combine(rutaGuardado, nombrePDF), FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Agregar encabezado
                AddHeader(doc);

                // Agregar título
                AddTitle(doc);

                // Agregar detalles del salón
                AddDetails(doc);

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
                                PdfPTable detailsTable = new PdfPTable(2); // 2 columnas para nombre de columna y valor
                                detailsTable.WidthPercentage = 100;
                                detailsTable.SpacingBefore = 10;

                                foreach (DataColumn column in dataTable.Columns)
                                {
                                    // Agregar nombre de columna en una celda
                                    PdfPCell cellHeader = new PdfPCell(new Phrase(column.ColumnName, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK)));
                                    cellHeader.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cellHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    cellHeader.Padding = 5;
                                    cellHeader.FixedHeight = 20f;
                                    cellHeader.NoWrap = true;
                                    detailsTable.AddCell(cellHeader);

                                    // Agregar valores en celdas adicionales
                                    foreach (DataRow dataRow in dataTable.Rows)
                                    {
                                        PdfPCell cellValue = new PdfPCell(new Phrase(dataRow[column].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK)));
                                        cellValue.HorizontalAlignment = Element.ALIGN_CENTER;
                                        cellValue.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        cellValue.Padding = 5;
                                        cellValue.FixedHeight = 20f;
                                        cellValue.NoWrap = true;
                                        detailsTable.AddCell(cellValue);
                                    }
                                }

                                doc.Add(detailsTable);
                            }
                        }
                    }
                }

                // Agregar footer
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
            cell.FixedHeight = 20f;
            cell.NoWrap = true;

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
            Paragraph footer = new Paragraph("Gracias por su preferencia. Para consultas, contacte a nuestro número de teléfono. Mary Stilist", FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK));
            footer.Alignment = Element.ALIGN_CENTER;
            footer.SpacingBefore = 10;

            doc.Add(footer);
        }

        private static void AddDetails(Document doc)
        {
            Paragraph title = new Paragraph("Mary Stilist. Barva, Heredia, Costa Rica. (506) 6060 2509", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 15, BaseColor.BLUE));
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 10;

            doc.Add(title);
        }
    }
}
