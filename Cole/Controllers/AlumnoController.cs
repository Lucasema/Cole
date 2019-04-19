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
    public class AlumnoController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Alumno
        public ActionResult Index()
        {
            var alumno = db.Alumno.Include(a => a.Persona).Include(a => a.Tutor);
            return View(alumno.ToList());
        }

        // GET: Alumno/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumno.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // GET: Alumno/Create
        public ActionResult Create()
        {
            ViewBag.Dni = new SelectList(db.Persona, "Dni", "Contraseña");
            ViewBag.DniTutor = new SelectList(db.Tutor, "Dni", "Ocupacion");
            return View();
        }

        // POST: Alumno/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                db.Persona.Add(alumno.Persona);
                db.Tutor.Add(alumno.Tutor);

                alumno.Dni = alumno.Persona.Dni;
                alumno.DniTutor = alumno.Tutor.Dni;

                db.Alumno.Add(alumno);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dni = new SelectList(db.Persona, "Dni", "Contraseña", alumno.Dni);
            ViewBag.DniTutor = new SelectList(db.Tutor, "Dni", "Ocupacion", alumno.DniTutor);
            return View(alumno);
        }

        // GET: Alumno/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumno.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dni = new SelectList(db.Persona, "Dni", "Contraseña", alumno.Dni);
            ViewBag.DniTutor = new SelectList(db.Tutor, "Dni", "Ocupacion", alumno.DniTutor);
            return View(alumno);
        }

        // POST: Alumno/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Dni,DniTutor")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alumno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dni = new SelectList(db.Persona, "Dni", "Contraseña", alumno.Dni);
            ViewBag.DniTutor = new SelectList(db.Tutor, "Dni", "Ocupacion", alumno.DniTutor);
            return View(alumno);
        }

        // GET: Alumno/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumno.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // POST: Alumno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alumno alumno = db.Alumno.Find(id);
            db.Alumno.Remove(alumno);
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
