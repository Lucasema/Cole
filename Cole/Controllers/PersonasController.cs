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
    public class PersonasController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Personas
        public ActionResult Index()
        {
            var persona = db.Persona.Include(p => p.Alumno).Include(p => p.Profesor).Include(p => p.Tutor);
            return View(persona.ToList());
        }

        // GET: Personas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // GET: Personas/Create
        public ActionResult Create()
        {
            ViewBag.Dni = new SelectList(db.Alumno, "Dni", "Dni");
            ViewBag.Dni = new SelectList(db.Profesor, "Dni", "Dni");
            ViewBag.Dni = new SelectList(db.Tutor, "Dni", "Ocupacion");
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Dni,Cuil,TelCelular,TelFijo,Contraseña,Sexo,Domicilio,Nacionalidad,Nombre,Apellido,FechaNacimiento")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db.Persona.Add(persona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dni = new SelectList(db.Alumno, "Dni", "Dni", persona.Dni);
            ViewBag.Dni = new SelectList(db.Profesor, "Dni", "Dni", persona.Dni);
            ViewBag.Dni = new SelectList(db.Tutor, "Dni", "Ocupacion", persona.Dni);
            return View(persona);
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dni = new SelectList(db.Alumno, "Dni", "Dni", persona.Dni);
            ViewBag.Dni = new SelectList(db.Profesor, "Dni", "Dni", persona.Dni);
            ViewBag.Dni = new SelectList(db.Tutor, "Dni", "Ocupacion", persona.Dni);
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Dni,Cuil,TelCelular,TelFijo,Contraseña,Sexo,Domicilio,Nacionalidad,Nombre,Apellido,FechaNacimiento")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dni = new SelectList(db.Alumno, "Dni", "Dni", persona.Dni);
            ViewBag.Dni = new SelectList(db.Profesor, "Dni", "Dni", persona.Dni);
            ViewBag.Dni = new SelectList(db.Tutor, "Dni", "Ocupacion", persona.Dni);
            return View(persona);
        }

        // GET: Personas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Persona.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Persona persona = db.Persona.Find(id);
            db.Persona.Remove(persona);
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
