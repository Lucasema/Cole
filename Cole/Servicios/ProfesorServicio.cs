using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cole.Models;
using System.Data.Entity;

namespace Cole.Servicios
{
    public static class ProfesorServicio
    {
        private static ColegioEntities db = new ColegioEntities();
        public static List<Profesor> Buscar(string campo, string valor)
        {
            List<Profesor> profesores = null;

            if (campo == "dni")
            {
                // en este caso, se podria encontrar erroneamente una cadena de caracteres y seria incorrecto

                int dni = Int32.Parse(valor);


                var queryProfesor = db.Profesor.Include(p => p.Persona).Where(x => x.Dni == dni);

                profesores = queryProfesor.ToList();


            }
            else
            {

                var queryProfesor = db.Profesor.Include(p => p.Persona).Where(x => x.Persona.Apellido == valor);

                profesores = queryProfesor.ToList();

            }

            return profesores;
        }
    }
}