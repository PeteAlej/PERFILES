using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Element;
using PERFILES.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace PERFILES.Controllers
{
    public class EmpleadosController : Controller
    {
        // GET: Empleados/Empleado
        public ActionResult Empleados()
        {
            List<Empleados> empleados = new List<Empleados>();

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";
            string sqlQuery = "SELECT * FROM Empleados;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Empleados empleado = new Empleados
                    {
                        ID = (int)reader["ID"],
                        Nombres = reader["NOMBRES"].ToString(),
                        DPI = (long)reader["DPI"],
                        FechaNacimiento = (DateTime)reader["NACIMIENTO"],
                        Genero = (string)reader["GENERO"],
                        FechaIngreso = (DateTime)reader["INGRESO_EMPRESA"],
                        Edad = (int)reader["EDAD"],
                        Direccion = (string)reader["DIRECCION"],
                        NIT = (int)reader["NIT"],
                        Departamento = (string)reader["DEPARTAMENTO"]
                    };
                    empleados.Add(empleado);
                }
                reader.Close();
            }
            return View(empleados);
        }
        // GET: Registro
        public ActionResult Registrar()
        {
            return View("~/Views/Empleados/Registrar.cshtml");
        }


        [HttpPost]
        public ActionResult AgregarEmpleado(FormCollection form)
        {
            string nombres = form["input-nombres"].ToUpper();
            long dpi = Convert.ToInt64(form["input-dpi"]);
            DateTime nacimiento = DateTime.Parse(form["fecha-nacimiento"]);
            string genero = form["seleccion-genero"].ToUpper();
            DateTime ingreso = DateTime.Parse(form["fecha-ingreso"]);
            int edad = Convert.ToInt16(form["calculo-edad"]);
            string direccion = form["input-direccion"].ToUpper();
            string nit = form["input-nit"];
            if (nit != "")
            {
                Convert.ToInt32(nit);
            }
            string departamento = form["seleccion-departamento"].ToUpper();
            string boton = form["boton-post"].ToUpper();

        // Connection string to your SQL Server database
        string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "USE dbPERFILES; INSERT INTO Empleados (NOMBRES, DPI, NACIMIENTO, GENERO, INGRESO_EMPRESA, EDAD, DIRECCION, NIT, DEPARTAMENTO) " +
                    "VALUES (@nombres, @dpi, @nacimiento, @genero, @ingreso, @edad, @direccion, @nit, @departamento)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@nombres", nombres);
                    command.Parameters.AddWithValue("@dpi", dpi);
                    command.Parameters.AddWithValue("@nacimiento", nacimiento);
                    command.Parameters.AddWithValue("@genero", genero);
                    command.Parameters.AddWithValue("@ingreso", ingreso);
                    command.Parameters.AddWithValue("@edad", edad);
                    command.Parameters.AddWithValue("@direccion", direccion);
                    command.Parameters.AddWithValue("@nit", nit);
                    command.Parameters.AddWithValue("@departamento", departamento);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // Redirect to a success page or perform any other action
            return RedirectToAction("Success");
        }

        // GET: YourController/Success
        public ActionResult Success()
        {
            return View("~/Views/Empleados/Registrar.cshtml");
        }

        [HttpPost]
        public ActionResult EliminarEmpleado(FormCollection form)
        {
            string firstKey = form.AllKeys[0];
            string dynamicId = firstKey.Substring("eliminar-".Length);
            int id = Convert.ToInt16(dynamicId);

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "USE dbPERFILES; DELETE FROM Empleados WHERE ID = @id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Prevención de inyecciones de SQL
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

            }

            return RedirectToAction("Empleados") ;
        }

        //GET
        public ActionResult Modificar(int id)
        {
            Empleados empleado = new Empleados();

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";
            string sqlQuery = "USE dbPERFILES; SELECT * FROM Empleados WHERE ID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", Convert.ToInt16(id));
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        empleado.ID = Convert.ToInt16(reader["ID"]);
                        empleado.Nombres = reader["NOMBRES"].ToString();
                        empleado.DPI = (long)reader["DPI"];
                        empleado.FechaNacimiento = (DateTime)reader["NACIMIENTO"];
                        empleado.Genero = (string)reader["GENERO"];
                        switch (empleado.Genero)
                        {
                            case "MASCULINO":
                                empleado.Genero = "Masculino";
                                break;
                            case "FEMENINO":
                                empleado.Genero = "Femenino";
                                break;
                        }
                        empleado.FechaIngreso = (DateTime)reader["INGRESO_EMPRESA"];
                        empleado.Edad = (int)reader["EDAD"];
                        empleado.Direccion = (string)reader["DIRECCION"];
                        empleado.NIT = (int)reader["NIT"];
                        empleado.Departamento = (string)reader["DEPARTAMENTO"];
                        switch (empleado.Departamento)
                        {
                            case "VENTAS":
                                empleado.Departamento = "Ventas";
                                break;
                            case "RECURSOS HUMANOS":
                                empleado.Departamento = "Recursos Humanos";
                                break;
                            case "IT":
                                empleado.Departamento = "IT";
                                break;
                            case "LEGAL":
                                empleado.Departamento = "Legal";
                                break;
                        }
                    }
                    reader.Close();
                }
            }
            return View("~/Views/Empleados/Modificar.cshtml", empleado);
        }

        [HttpPost]
        public ActionResult Modificar(string id)
        {
            /*Empleados empleado = new Empleados();

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";
            string sqlQuery = "USE dbPERFILES; SELECT * FROM Empleados WHERE ID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", Convert.ToInt16(id));
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        empleado.ID = Convert.ToInt16(reader["ID"]);
                        empleado.Nombres = reader["NOMBRES"].ToString();
                        empleado.DPI = (long)reader["DPI"];
                        empleado.FechaNacimiento = (DateTime)reader["NACIMIENTO"];
                        empleado.Genero = (string)reader["GENERO"];
                        switch (empleado.Genero)
                        {
                            case "MASCULINO":
                                empleado.Genero = "Masculino";
                                break;
                            case "FEMENINO":
                                empleado.Genero = "Femenino";
                                break;
                        }
                        empleado.FechaIngreso = (DateTime)reader["INGRESO_EMPRESA"];
                        empleado.Edad = (int)reader["EDAD"];
                        empleado.Direccion = (string)reader["DIRECCION"];
                        empleado.NIT = (int)reader["NIT"];
                        empleado.Departamento = (string)reader["DEPARTAMENTO"];
                        switch (empleado.Departamento)
                        {
                            case "VENTAS":
                                empleado.Departamento = "Ventas";
                                break;
                            case "RECURSOS HUMANOS":
                                empleado.Departamento = "Recursos Humanos";
                                break;
                            case "IT":
                                empleado.Departamento = "IT";
                                break;
                            case "LEGAL":
                                empleado.Departamento = "Legal";
                                break;
                        }
                    }
                    reader.Close();
                }
            }*/
            //return View("~/Views/Empleados/Modificar.cshtml");
            return RedirectToAction("Modificar", new { id = Convert.ToInt16(id) });
        }

        [HttpPost]
        public ActionResult Filtrar (FormCollection form)
        {
            List<Empleados> empleados = new List<Empleados>();
            string[] filtro_selector = new string[6];
            filtro_selector[0] = form["filtro-selector"];       //Selector para seleccionar el tipo de filtro.
            filtro_selector[1] = form["buscador-num"];          // Input exclusivamente para números.
            filtro_selector[2] = form["buscador-texto"];        // Input exclusivamente para texto.
            filtro_selector[3] = form["filtro-departamento"];   // Selector de departamentos
            filtro_selector[4] = form["fecha-inf"];             // Selector de valor de rango inicial de fecha de ingreso.
            filtro_selector[5] = form["fecha-sup"];             // Selector de valor de rango final de fecha de ingreso.

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "";
                switch (filtro_selector[0])
                {
                    case "ID":
                        query = "USE dbPERFILES; SELECT * FROM Empleados WHERE ID = @var;";
                        if (filtro_selector[1] == "")
                        {
                            filtro_selector[1] = "0";
                        }
                        Convert.ToInt16(filtro_selector[1]);
                        break;
                    case "Nombres":
                        query = "USE dbPERFILES; SELECT * FROM Empleados WHERE NOMBRES LIKE @var;";
                        if (int.TryParse(filtro_selector[2], out int result))
                        {
                            break;
                        }
                        else
                        {
                            filtro_selector[2] = filtro_selector[2].ToUpper();
                        }
                        break;
                    case "DPI":
                        query = "USE dbPERFILES; SELECT * FROM Empleados WHERE DPI = @var;";
                        if (filtro_selector[1] == "")
                        {
                            filtro_selector[1] = "0";
                        }
                        Convert.ToInt64(filtro_selector[1]);
                        break;
                    case "Departamento":
                        query = "USE dbPERFILES; SELECT * FROM Empleados WHERE DEPARTAMENTO LIKE @var;";
                        filtro_selector[3] = filtro_selector[3].ToUpper();
                        break;
                    case "Fecha de Ingreso":
                        query = "USE dbPERFILES; SELECT * FROM Empleados WHERE NOT (INGRESO_EMPRESA > @fecha_sup OR INGRESO_EMPRESA < @fecha_inf);";
                        break;
                }
                SqlCommand command = new SqlCommand(query, connection);
                if (filtro_selector[0] == "Nombres")
                {
                    command.Parameters.AddWithValue("@var", "%" + filtro_selector[2] + "%");
                }
                else if (filtro_selector[0] == "DPI" || filtro_selector[0] == "ID")
                {
                    command.Parameters.AddWithValue("@var", filtro_selector[1]);
                }
                else if (filtro_selector[0] == "Departamento")
                {
                    command.Parameters.AddWithValue("@var", filtro_selector[3]);
                }
                else if (filtro_selector[0] == "Fecha de Ingreso")
                {
                    command.Parameters.AddWithValue("@fecha_sup", filtro_selector[5]);
                    command.Parameters.AddWithValue("@fecha_inf", filtro_selector[4]);
                }
                connection.Open();
                command.ExecuteNonQuery();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Empleados empleado = new Empleados
                    {
                        ID = (int)reader["ID"],
                        Nombres = reader["NOMBRES"].ToString(),
                        DPI = (long)reader["DPI"],
                        FechaNacimiento = (DateTime)reader["NACIMIENTO"],
                        Genero = (string)reader["GENERO"],
                        FechaIngreso = (DateTime)reader["INGRESO_EMPRESA"],
                        Edad = (int)reader["EDAD"],
                        Direccion = (string)reader["DIRECCION"],
                        NIT = (int)reader["NIT"],
                        Departamento = (string)reader["DEPARTAMENTO"]
                    };
                    empleados.Add(empleado);
                }
                reader.Close();
            }
            return View("~/Views/Empleados/Empleados.cshtml", empleados);
        }

        [HttpPost]
        public ActionResult RestablecerFiltro()
        {
            return RedirectToAction("Empleados");
        }

        public ActionResult Reporte()
        {
            List<Empleados> empleados = new List<Empleados>();

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";
            string sqlQuery = "SELECT TOP 10 * FROM Empleados;";
            DataTable dataTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery, conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataTable);
            }

            MemoryStream stream = new MemoryStream();   // Instancia un espacio de memoria
            PdfWriter writer = new PdfWriter(stream);   // 
            PdfDocument pdf = new PdfDocument(writer);
            Document doc = new Document(pdf);

            PageSize pageSize = PageSize.A4.Rotate();
            pdf.SetDefaultPageSize(pageSize);

            Paragraph titulo = new Paragraph("Reporte de primeros 10 empleados");
            titulo.SetTextAlignment(TextAlignment.CENTER);
            titulo.SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
            titulo.SetMarginBottom(10.0f);
            titulo.SetFontSize(18);
            doc.Add(titulo);

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