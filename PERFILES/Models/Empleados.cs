using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PERFILES.Models
{

    public class Empleados
    {
        public int ID { get; set; }
        public string Nombres { get; set; }
        public long DPI { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int Edad { get; set; }
        public string Direccion { get; set; }
        public int NIT { get; set; }
        public string Departamento { get; set; }
    }
}