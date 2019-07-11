using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cole.Models;

namespace Cole.Controllers
{
    [Filters.Autorizar(Roles = "Administrador")]
    public class InstitucionController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Institucion
        public ActionResult Index()
        {
            var institucion = db.Institucion.Include(i => i.Persona);
            return View(institucion.ToList());
        }

        // GET: Institucion/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institucion institucion = db.Institucion.Find(id);
            if (institucion == null)
            {
                return HttpNotFound();
            }
            return View(institucion);
        }

        // GET: Institucion/Create
        public ActionResult Create()
        {
            ViewBag.DniAdministrador = new SelectList(db.Persona, "Dni", "Cuil");
            return View();
        }

        // POST: Institucion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Numero,Categoria,NumeroTelefono,Direccion,DniAdministrador")] Institucion institucion)
        {
            if (ModelState.IsValid)
            {
                db.Institucion.Add(institucion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DniAdministrador = new SelectList(db.Persona, "Dni", "Cuil", institucion.DniAdministrador);
            return View(institucion);
        }

        // GET: Institucion/Edit/5
        public ActionResult Edit()
        {
            
            Institucion institucion = db.Institucion.First();

            if (institucion == null)
            {
                return HttpNotFound();
            }

            return View(institucion);
        }

        // POST: Institucion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Numero,Categoria,NumeroTelefono,Direccion,DniAdministrador")] Institucion institucion)
        {
            if(institucion.DniAdministrador != null)
            {
                if (db.Persona.Find(institucion.DniAdministrador) != null)
                {

                    if (ModelState.IsValid)
                    {
                        db.Entry(institucion).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Edit");
                    }

                }
                else
                {
                    @ViewBag.errorDni = "La persona no se encuentra registrada.";
                }
            }
            else
            {
                @ViewBag.errorDni = "Este campo es obligatorio.";
            }
            
            


            return View(institucion);
        }

        // GET: Institucion/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institucion institucion = db.Institucion.Find(id);
            if (institucion == null)
            {
                return HttpNotFound();
            }
            return View(institucion);
        }

        // POST: Institucion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Institucion institucion = db.Institucion.Find(id);
            db.Institucion.Remove(institucion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
