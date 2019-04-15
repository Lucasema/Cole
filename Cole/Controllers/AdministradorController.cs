using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cole.Controllers
{
    public class AdministradorController : Controller
    {
        // GET: Administrador


        public ActionResult Index()
        {
            HttpContext.User.IsInRole("Administrador");
            bool a = HttpContext.User.Identity.IsAuthenticated;
            string un = (String)Session["UserName"];
            return View();
        }
    }
}