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

            if(p.Dni != 0)
            {

                if (p.Contraseña != null && p.Contraseña != "")
                {

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

                                return RedirectToAction("Index", "Administrador");
                            }
                            else if (LoginServicio.EsAlumno(persona.Dni))
                            {
                                HttpContext.Session["Role"] = "Alumno";
                                return View();
                            }
                            else if (LoginServicio.EsProfesor(persona.Dni))
                            {
                                HttpContext.Session["Role"] = "Profesor";
                                HttpContext.Session["Dni"] = persona.Dni;

                                return RedirectToAction("CursosPorProfe", "Curso");
                            }
                        }

                        ViewBag.error = "D.N.I. o contraseña incorrecta.";
                    }
                    catch (Exception e)
                    {
                        ViewBag.error = "D.N.I. o contraseña incorrecta.";
                    }


                }
                else
                {
                    ViewBag.errorContraseña = "Ingrese su contraseña";
                }
            }
            else
            {
                ViewBag.errorDni = "Ingrese un dni válido";
            }


            return View("Index", p);
            
           

            
        }
    }
}