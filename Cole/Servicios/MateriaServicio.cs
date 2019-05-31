using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using System.Web;
using Cole.Models;
namespace Cole.Servicios
{
    public static class MateriaServicio
    {
        private static ColegioEntities db = new ColegioEntities();

        public static List<Materia> Buscar(string valor)
        {
            List<Materia> materias = null;

            var queryAlumnos = db.Materia.Where(x => x.Nombre == valor);

            materias = queryAlumnos.ToList();


            return materias;
        }


        public static bool Existe(string Nombre)
        {
            Materia materia = db.Database.SqlQuery<Materia>("SELECT * FROM Materia WHERE Nombre = @p0", Nombre).FirstOrDefault();

            return materia != null;
        }

    }
}