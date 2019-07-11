using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cole.Models;

namespace Cole.Servicios
{
    [Filters.Autorizar(Roles = "Administrador")]
    public class TutorsController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Tutors
        public ActionResult Index()
        {
            var tutor = db.Tutor.Include(t => t.Persona);
            return View(tutor.ToList());
        }

        [HttpPost]
        public ActionResult Index(string campo, string valor)
        {
            if (valor != "")
            {
                List<Tutor> tutores = TutorServicio.Buscar(campo, valor);
                return View(tutores);
            }
            return Index();
        }

        // GET: Tutors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutor.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // GET: Tutors/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Tutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tutor tutor)
        {
            tutor.Dni = tutor.Persona.Dni;
            

            if (ModelState.IsValid)
            {
                db.Tutor.Add(tutor);

                try
                {
                    db.SaveChanges();
                }
                catch(DbUpdateException e)
                {

                    Persona result = db.Database.SqlQuery<Persona>("SELECT * FROM Persona WHERE Persona.Dni = @p0", parameters: tutor.Dni).FirstOrDefault();

                    if (result != null)
                    {
                        ViewBag.keyDuplicada = "Ya existe una persona con ese D.N.I.";
                        return View(tutor);
                    }

                    throw e;
                }
                return RedirectToAction("Index");
            }

            ViewBag.Dni = new SelectList(db.Persona, "Dni", "Cuil", tutor.Dni);
            return View(tutor);
        }

        // GET: Tutors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutor.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dni = new SelectList(db.Persona, "Dni", "Cuil", tutor.Dni);
            return View(tutor);
        }

        // POST: Tutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Tutor tutor)
        {
            tutor.Persona.Dni = tutor.Dni;
            
            if (ModelState.IsValid)
            {
                db.Entry(tutor).State = EntityState.Modified;
                db.Entry(tutor.Persona).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tutor);
        }

        // GET: Tutors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutor.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // POST: Tutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tutor tutor = db.Tutor.Find(id);
            Persona p = db.Persona.Find(id);

            db.Tutor.Remove(tutor);
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
