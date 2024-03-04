using PERFILES.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PERFILES.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Empleados()
        {
            List<Empleados> empleados = new List<Empleados>();

            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dbPERFILES;Integrated Security=True";
            string sqlQuery = "SELECT * FROM Empleados";

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
    }
}