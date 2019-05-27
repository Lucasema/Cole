using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using System.Web;
using Cole.Models;
namespace Cole.Servicios
{
    public static class TutorServicio
    {
        private static ColegioEntities db = new ColegioEntities();

        public static List<Tutor> Buscar(string campo, string valor)
        {
            List<Tutor> tutores = null;

            if (campo == "dni")
            {
                // en este caso, se podria encontrar erroneamente una cadena de caracteres y seria incorrecto

                int dni = Int32.Parse(valor);
                var queryTutores = db.Tutor.Include(a => a.Persona).Where(x => x.Dni == dni);

                tutores = queryTutores.ToList();


            }
            else
            {

                var queryTutores = db.Tutor.Include(a => a.Persona).Where(x => x.Persona.Apellido == valor);

                tutores = queryTutores.ToList();

            }

            return tutores;
        }

    }
}