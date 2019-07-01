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
    [Filters.Autorizar(Roles ="Profesor")]
    public class EvaluacionController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Evaluacion
        public ActionResult Index()
        {
            int Dni = (int)HttpContext.Session["Dni"];

            List<Evaluacion> evaluaciones = db.Evaluacion.SqlQuery("SELECT * FROM Evaluacion WHERE IdMateria IN (SELECT IdMateria FROM Dicta WHERE DniProfesor = @p0)", Dni).ToList();

            foreach(Evaluacion e in evaluaciones)
            {
                e.Materia = db.Materia.Find(e.IdMateria);
            }


            return View(evaluaciones);
        }

        // GET: Evaluacion/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluacion evaluacion = db.Evaluacion.Find(id);
            if (evaluacion == null)
            {
                return HttpNotFound();
            }
            return View(evaluacion);
        }

        // GET: Evaluacion/Create
        public ActionResult Create()
        {
            ViewBag.IdMateria = new SelectList(db.Materia, "Id", "Nombre");
            return View();
        }

        // POST: Evaluacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Fecha,Titulo,Tipo,IdMateria")] Evaluacion evaluacion)
        {
            if (ModelState.IsValid)
            {
                db.Evaluacion.Add(evaluacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdMateria = new SelectList(db.Materia, "Id", "Nombre", evaluacion.IdMateria);
            return View(evaluacion);
        }

        // GET: Evaluacion/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluacion evaluacion = db.Evaluacion.Find(id);
            if (evaluacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMateria = new SelectList(db.Materia, "Id", "Nombre", evaluacion.IdMateria);
            return View(evaluacion);
        }

        // POST: Evaluacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Fecha,Titulo,Tipo,IdMateria")] Evaluacion evaluacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evaluacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdMateria = new SelectList(db.Materia, "Id", "Nombre", evaluacion.IdMateria);
            return View(evaluacion);
        }

        // GET: Evaluacion/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluacion evaluacion = db.Evaluacion.Find(id);
            if (evaluacion == null)
            {
                return HttpNotFound();
            }
            return View(evaluacion);
        }

        // POST: Evaluacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            Evaluacion evaluacion = db.Evaluacion.Find(id);
            db.Evaluacion.Remove(evaluacion);
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
