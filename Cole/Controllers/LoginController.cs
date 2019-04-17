using Cole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Cole.Servicios;
using System.Threading;

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

            if(p.Dni == 0)
            {
                ViewBag.errorDni = "Ingrese un dni válido";
            }

            if(p.Contraseña == null || p.Contraseña == "")
            {
                ViewBag.errorContraseña = "Ingrese su contraseña"; 
            }
            

            try
            {
                //busca la persona con el dni y contraseña dada
                var persona = db.Persona.Where(x => x.Dni == p.Dni && x.Contraseña == p.Contraseña).First();
                if (persona != null)
                {
                    HttpContext.Session["IsAuthenticated"] = true;
               
                    //si es el administrador
                    if (LoginServicio.EsAdministrador(persona.Dni))
                    {
                        HttpContext.Session["Role"] = "Administrador";
                        
                        return RedirectToAction("Index","Administrador");
                    }
                    else if (LoginServicio.EsAlumno(persona.Dni))
                    {
                        HttpContext.Session["Role"] = "Alumno";
                        return View();
                    }
                    else if (LoginServicio.EsProfesor(persona.Dni))
                    {
                        HttpContext.Session["Role"] = "Profesor";
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