using PERFILES.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PERFILES.Controllers
{
    public class ModificarController : Controller
    {

        [HttpPost]
        public ActionResult CargarModificaciones(FormCollection form)
        {
            int id = Convert.ToInt16(form["id-empleado"]);
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
            //string boton = form["boton-post"];

            // Connection string to your SQL Server database
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "USE dbPERFILES; UPDATE Empleados SET NOMBRES = @nombres, DPI = @dpi, NACIMIENTO = @nacimiento, GENERO = @genero, " +
                        "INGRESO_EMPRESA = @ingreso, EDAD = @edad, DIRECCION = @direccion, NIT = @nit, DEPARTAMENTO = @departamento " +
                        "WHERE ID = @id";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@id", id);
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

            return RedirectToAction("Empleados", "Empleados");
        }
    }
}