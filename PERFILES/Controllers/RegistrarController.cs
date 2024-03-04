using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PERFILES.Controllers
{
    public class RegistrarController : Controller
    {
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
            string boton = form["boton-post"];

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
    }
}