using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

using System.Web;
using Cole.Models;
namespace Cole.Servicios
{
    public static class CursoServicio
    {
        private static ColegioEntities db = new ColegioEntities();

        public static List<Curso> Buscar(int nro, string division)
        {
            
            var queryAlumnos = db.Curso.Where(x => x.Nro == nro && x.Division == division);

            List<Curso> cursos = queryAlumnos.ToList();

            return cursos;
        }

    }
}