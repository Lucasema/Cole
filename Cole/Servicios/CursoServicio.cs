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


        public static bool Existe(int nro, string division)
        {

            Curso resultado = db.Database.SqlQuery<Curso>("SELECT * FROM Curso WHERE Nro = @p0 and Division = @p1",
                nro, division).FirstOrDefault();

            return resultado != null;
        }



        public static bool Asiste(int dniAlumno, int idCurso, int año)
        {


            Asiste resultado = db.Database.SqlQuery<Asiste>("SELECT *" +
                " FROM Asiste" +
                " WHERE DniAlumno = @p0 and año = @p1 and IdCurso = @p2", dniAlumno, DateTime.Parse("01/01/"+año.ToString()), idCurso).FirstOrDefault();

            return resultado != null;

        }
    }

    


}