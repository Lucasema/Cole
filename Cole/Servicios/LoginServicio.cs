using Cole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cole.Servicios
{
    public static class LoginServicio
    {
        static ColegioEntities db = new ColegioEntities();

        public static Boolean EsAdministrador(int dni)
        {
            
            //busca el dni del administrador
            var dniAdministrador = db.Database.SqlQuery<int>(
                "Select DniAdministrador " +
                "From Institucion " +
                "Where Id=1"
                ).First();

            if (dniAdministrador == dni)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean EsAlumno(int dni)
        {

            //busca el alumno, si lo encuentra asigna 1
            var esAlumno = db.Database.SqlQuery<int>(
                "select count(Dni) " +
                "from Alumno " +
                "where Dni = @p0",dni
                ).First();

            if (esAlumno == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean EsProfesor(int dni)
        {

            //busca el profesor, si lo encuentra asigna 1
            var esProfesor = db.Database.SqlQuery<int>(
                "select count(Dni) " +
                "from Profesor " +
                "where Dni = @p0", dni
                ).First();

            if (esProfesor == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}