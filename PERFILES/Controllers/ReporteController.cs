using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PERFILES.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PERFILES.Controllers
{
    public class ReporteController : Controller
    {
        [HttpPost]
        public ActionResult Filtro(FormCollection form)
        {
            string fechaInf = form["fecha-inf"];
            string fechaSup = form["fecha-sup"];
            string departamento = form["selector-dep"].ToUpper();

            DataTable dataTable = new DataTable();

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";
            string query = "";

            if ((fechaInf == "" && fechaSup == "") || (fechaInf == "" && fechaSup != "" ))
            {
                fechaInf = "Desde inicios";
                fechaSup = DateTime.Now.Date.ToString("yyyy-MM-dd");
                query = "USE dbPERFILES; SELECT * FROM Empleados WHERE DEPARTAMENTO = @var;";
            }
            else if (fechaInf != "" || fechaSup == "")
            {
                fechaSup = DateTime.Now.Date.ToString("yyyy-MM-dd");
                query = "USE dbPERFILES; SELECT * FROM Empleados WHERE DEPARTAMENTO = @var AND INGRESO_EMPRESA BETWEEN @fechaInf AND CAST(GETDATE() AS Date)";
            }
            else if ((fechaInf != "" && fechaSup != "") && departamento != "")
            {
                fechaSup = DateTime.ParseExact(fechaSup, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                query = "USE dbPERFILES; SELECT * FROM Empleados WHERE DEPARTAMENTO = @var AND INGRESO_EMPRESA BETWEEN @fechaInf AND @fechaSup)";
            }
            else if ((fechaInf != "" && fechaSup != "") && departamento == "Todos")
            {
                fechaSup = DateTime.ParseExact(fechaSup, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                query = "USE dbPERFILES; SELECT * FROM Empleados WHERE INGRESO_EMPRESA BETWEEN @fechaInf AND @fechaSup)";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@fechaInf", fechaInf);
                command.Parameters.AddWithValue("@fechaSup", fechaSup);
                command.Parameters.AddWithValue("@var", departamento);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataTable);
            }

            MemoryStream stream = new MemoryStream();   // Instancia un espacio de memoria
            PdfWriter writer = new PdfWriter(stream);   // 
            PdfDocument pdf = new PdfDocument(writer);
            Document doc = new Document(pdf);

            PageSize pageSize = PageSize.A4.Rotate();
            pdf.SetDefaultPageSize(pageSize);

            Paragraph titulo = new Paragraph("Reporte de empleados");
            Paragraph subtitulo = new Paragraph(fechaInf + " al " + fechaSup);
            titulo.SetTextAlignment(TextAlignment.CENTER);
            subtitulo.SetTextAlignment(TextAlignment.CENTER);
            titulo.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
            subtitulo.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
            titulo.SetFontSize(18);
            subtitulo.SetFontSize(18);
            subtitulo.SetMarginBottom(10.0f);
            doc.Add(titulo);
            doc.Add(subtitulo);

            Table table = new Table(dataTable.Columns.Count);

            // Agregar los titulos de cada columna
            foreach (DataColumn column in dataTable.Columns)
            {
                table.AddHeaderCell(new Cell().Add(new Paragraph(column.ColumnName)));
            }

            // Agregar filas y las celdas a la tabla
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    table.AddCell(item.ToString());
                }
            }

            // Add the table to the document
            doc.Add(table);

            doc.Close();

            // El archivo PDF ya se creo en memoria, guardado en la variable stream de tipo MemoryStream.


            byte[] pdfBytes = stream.ToArray(); //Enlace del archivo PDF codificado en un arreglo de bytes, esto
                                                // para simplificar el envío de datos al View y luego convertirlos
                                                // a un formato legible por el iframe.

            string pdfString = Convert.ToBase64String(pdfBytes);

            return View("~/Views/Empleados/Reporte.cshtml", pdfBytes);
        }
    }
}