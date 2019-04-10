using Cole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cole.Servicios;

namespace Cole.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Persona p, string returnUrl)
        {
            ColegioEntities db = new ColegioEntities();
            try
            {
                //busca la persona con el dni y contraseña dada
                var persona = db.Persona.Where(x => x.Dni == p.Dni && x.Contraseña == p.Contraseña).First();
                if (persona != null)
                {


                    //si es el administrador
                    if (LoginServicio.EsAdministrador(persona.Dni))
                    {
                        //retorna vista de dministrador
                        return View();
                    }
                    else if (LoginServicio.EsAlumno(persona.Dni))
                    {
                        return View();
                    }
                    else if (LoginServicio.EsProfesor(persona.Dni))
                    {
                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.error = e.Message;
            }

            ViewBag.error = "D.N.I. o contraseña incorrecta.";
            return View("Index");
            
           

            
        }
    }
}