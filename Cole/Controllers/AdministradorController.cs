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


            [Filters.Autorizar(Roles ="Administrador")]
        public ActionResult Index()
        {
            return View();
        }

        [Filters.Autorizar(Roles = "Administrador")]
        public ActionResult ManualAdministrador()
        {
            return View();
        }
    }
}