using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cole.Models;
using Cole.Servicios;

namespace Cole.Controllers
{
    public class ProfesorsController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Profesors
        public ActionResult Index()
        {
            var profesor = db.Profesor.Include(p => p.Persona);
            return View(profesor.ToList());
        }

        [HttpPost]
        public ActionResult Index(string campo, string valor)
        {
            if (valor != "")
            {
                List<Profesor> profesores = ProfesorServicio.Buscar(campo, valor);
                return View(profesores);
            }
            return Index();
        }

        // GET: Profesors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesor.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // GET: Profesors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profesors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Profesor profesor)
        {
            foreach(Titulo item in profesor.Titulo)
            {
                item.Dni = profesor.Persona.Dni;
            }

            if (ModelState.IsValid)
            {
                profesor.Dni = profesor.Persona.Dni;

                db.Profesor.Add(profesor);
                //DbUpdateException si ya existe una persona en la base de datos con ese dni
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dni = new SelectList(db.Persona, "Dni", "Cuil", profesor.Dni);
            return View(profesor);
        }

        // GET: Profesors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesor.Include(p => p.Titulo).Where(p => p.Dni == id).First<Profesor>();

            if (profesor == null)
            {
                return HttpNotFound();
            }


            return View(profesor);
        }

        // POST: Profesors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Profesor profesor)
        {
            profesor.Dni = profesor.Persona.Dni;
            if (ModelState.IsValid)
            {
                db.Entry(profesor).State = EntityState.Modified;
                db.Entry(profesor.Persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profesor);
        }

        // GET: Profesors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profesor profesor = db.Profesor.Find(id);
            if (profesor == null)
            {
                return HttpNotFound();
            }
            return View(profesor);
        }

        // POST: Profesors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profesor profesor = db.Profesor.Find(id);
            Persona p = db.Persona.Find(id);
            List<Titulo> titulos = db.Titulo.Where(t => t.Dni == profesor.Dni).ToList<Titulo>();

            foreach(Titulo t in titulos)
            {
                db.Titulo.Remove(t);
            }

            db.Profesor.Remove(profesor);
            db.Persona.Remove(p);
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
