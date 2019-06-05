using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using System.Web;
using Cole.Models;
namespace Cole.Servicios
{
    public static class AlumnoServicio
    {
        private static ColegioEntities db = new ColegioEntities();

        public static List<Alumno> Buscar(string campo, string valor)
        {
            List<Alumno> alumnos = null;

            if (campo == "dni")
            {
                // en este caso, se podria encontrar erroneamente una cadena de caracteres y seria incorrecto

                int dni = Int32.Parse(valor);
                var queryAlumnos = db.Alumno.Include(a => a.Persona).Include(a => a.Tutor).Where(x => x.Dni == dni);

                alumnos = queryAlumnos.ToList();


            }
            else
            {

                var queryAlumnos = db.Alumno.Include(a => a.Persona).Include(a => a.Tutor).Where(x => x.Persona.Apellido == valor);

                alumnos = queryAlumnos.ToList();

            }

            return alumnos;
        }


        public static bool Existe(int dni)
        {
            Alumno alumno = db.Database.SqlQuery<Alumno>("SELECT * FROM Alumno WHERE Dni = @p0", dni).FirstOrDefault();

            return alumno != null;
        }
    }


    
}