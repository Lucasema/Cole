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
    public class CursoController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Curso
        public ActionResult Index()
        {
            CargarDropdowns();

            return View(db.Curso.ToList());
        }

        private void CargarDropdowns()
        {
            IEnumerable<SelectListItem> nro = db.Database.SqlQuery<Int32>("select distinct(Nro) from Curso order by Nro").Select(c => new SelectListItem
            {
                Value = c.ToString(),
                Text = c.ToString()

            });



            ViewBag.Numeros = nro;

            IEnumerable<SelectListItem> division = db.Database.SqlQuery<String>("select distinct(Division) from Curso order by Division").Select(c => new SelectListItem
            {
                Value = c,
                Text = c

            });

            ViewBag.Divisiones = division;

        }

        [HttpPost]
        public ActionResult Index(string nro, string division)
        {
            

            if (nro != null && division != null)
            {
                List<Curso> Cursos = CursoServicio.Buscar(Int32.Parse(nro), division);
                CargarDropdowns();
                return View(Cursos);
            }
            return Index();
        }

        // GET: Curso/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Curso.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // GET: Curso/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Curso/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Curso.Add(curso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(curso);
        }

        // GET: Curso/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Curso curso = db.Curso.Include(c => c.Asiste).Where(c => c.Id == id).First();

            List<Alumno> AlumnosDelCurso = db.Database.SqlQuery<Alumno>(
                "SELECT * " +
                "FROM Alumno " +
                "WHERE Alumno.Dni IN " +
                "(SELECT DniAlumno FROM Asiste WHERE Asiste.IdCurso == @p0", id.ToString()
                ).ToList<Alumno>();

            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Curso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nro,Division")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(curso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(curso);
        }

        // GET: Curso/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curso curso = db.Curso.Find(id);
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }

        // POST: Curso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curso curso = db.Curso.Find(id);
            db.Curso.Remove(curso);
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
