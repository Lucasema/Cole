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
    [Filters.Autorizar(Roles = "Profesor")]
    public class EvaluacionController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Evaluacion
        public ActionResult Index()
        {
            int Dni = (int)HttpContext.Session["Dni"];

            List<Evaluacion> evaluaciones = db.Evaluacion.SqlQuery("SELECT * FROM Evaluacion WHERE IdMateria IN (SELECT IdMateria FROM Dicta WHERE DniProfesor = @p0)", Dni).ToList();

            foreach (Evaluacion e in evaluaciones)
            {
                e.Materia = db.Materia.Find(e.IdMateria);
            }

            CargarDropdownMaterias(Dni);


            return View(evaluaciones);
        }

        public void CargarDropdownMaterias(int Dni)
        {
            IEnumerable<SelectListItem> materias = db.Materia.SqlQuery("SELECT * FROM Materia WHERE Id IN (SELECT IdMateria FROM Dicta Where DniProfesor = @p0)", Dni).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre.ToString()

            }).OrderBy(x => x.Text);

            ViewBag.materias = materias;
        }


        public ActionResult BuscarPorFecha(DateTime? desde, DateTime? hasta)
        {
            List<Evaluacion> evaluaciones = new List<Evaluacion>();


            if (desde != null)
            {
                ViewBag.desde = desde.Value.ToShortDateString();
            }

            if (hasta != null)
            {
                ViewBag.hasta = hasta.Value.ToShortDateString();

            }
            else
            {
                ViewBag.errorFechaHasta = "Ingrese la fecha.";
            }


            if (desde != null)
            {

                if (hasta != null)
                {

                    if (desde <= hasta)
                    {
                        evaluaciones = db.Evaluacion.Include(x => x.Materia).Where(x => x.Fecha >= desde && x.Fecha <= hasta).ToList();
                    }
                    else
                    {
                        ViewBag.errorIntervalo = "La fecha de inicio debe ser menor que la fecha de fin.";
                    }
                }

            }
            else
            {
                ViewBag.errorFechaDesde = "Ingrese la fecha.";
            }

            ViewBag.buscar = "fecha";

            CargarDropdownMaterias((int)HttpContext.Session["Dni"]);

            return View("Index", evaluaciones);
        }

        public ActionResult BuscarPorMateria(int materia)
        {
            List<Evaluacion> evaluaciones = db.Evaluacion.Include(x => x.Materia).Where(x => x.IdMateria == materia).ToList();

            if (evaluaciones == null)
            {
                evaluaciones = new List<Evaluacion>();
            }

            CargarDropdownMaterias((int)HttpContext.Session["Dni"]);

            return View("Index", evaluaciones);
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

            CargarDropdownMaterias((int)HttpContext.Session["Dni"]);

            return View();
        }




        // POST: Evaluacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Evaluacion evaluacion)
        {

            if (evaluacion.Fecha != DateTime.MinValue)
            {

                if (evaluacion.Titulo != null)
                {
                    if (db.Evaluacion.Where(x => x.Fecha == evaluacion.Fecha && x.Titulo == evaluacion.Titulo).FirstOrDefault() == null)
                    {
                        if (ModelState.IsValid)
                        {
                            db.Evaluacion.Add(evaluacion);
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {

                        ViewBag.ultimaFecha = evaluacion.Fecha.ToShortDateString();
                        ViewBag.errorEvaluacionExiste = "Ya existe la evaluación.";
                    }
                }
                else
                {
                    ViewBag.errorTitulo = "Este campo es obligatorio.";
                }

            }
            else
            {
                ViewBag.errorFecha = "Ingrese una fecha válida.";
            }


            CargarDropdownMaterias((int)HttpContext.Session["Dni"]);

            return View(evaluacion);
        }

        // GET: Evaluacion/Edit/5
        public ActionResult Edit(int? id)
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

            ViewBag.ultimaFecha = evaluacion.Fecha.ToShortDateString();

            CargarDropdownMaterias((int)HttpContext.Session["Dni"]);

            return View(evaluacion);
        }

        // POST: Evaluacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Evaluacion evaluacion)
        {


            if (evaluacion.Fecha != DateTime.MinValue)
            {

                if (evaluacion.Titulo != null)
                {
                    if (db.Evaluacion.Where(x => x.Fecha == evaluacion.Fecha && x.Titulo == evaluacion.Titulo).FirstOrDefault() == null)
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(evaluacion).State = EntityState.Modified;
                            db.SaveChanges();



                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {

                        ViewBag.ultimaFecha = evaluacion.Fecha.ToShortDateString();
                        ViewBag.errorEvaluacionExiste = "Ya existe la evaluación.";
                    }
                }
                else
                {
                    ViewBag.errorTitulo = "Este campo es obligatorio.";
                }

            }
            else
            {
                ViewBag.errorFecha = "Ingrese una fecha válida.";
            }

            
            ViewBag.IdMateria = new SelectList(db.Materia, "Id", "Nombre", evaluacion.IdMateria);
            return View(evaluacion);
        }

        // GET: Evaluacion/Delete/5
        public ActionResult Delete(int? id)
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

            db.Evaluacion.Remove(evaluacion);
            db.SaveChanges();

            return RedirectToAction("Index");
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


        public ActionResult Calificar(int idMateria, int idEvaluacion)
        {
            List<Persona> alumnos = db.Database.SqlQuery<Persona>("SELECT * FROM Persona WHERE Persona.Dni IN" +
                "(SELECT DniAlumno FROM Asiste WHERE Asiste.IdCurso IN " +
                "(SELECT IdCurso FROM Dicta WHERE Dicta.IdMateria = @p0 and Dicta.DniProfesor = @p1 and Dicta.año = @p2))", idMateria, HttpContext.Session["Dni"], DateTime.Parse("01/01/"+DateTime.Today.Year.ToString()) ).ToList();

            foreach(Persona p in alumnos)
            {
                p.Alumno = db.Alumno.Find(p.Dni);
            }



            if(alumnos == null)
            {
                alumnos = new List<Persona>();
            }

            List<SelectListItem> cursos = new List<SelectListItem>();

            cursos.Add(new SelectListItem { Value = "-1" , Text = "Todos" });


            IEnumerable<SelectListItem> cursos2 = db.Curso.SqlQuery("SELECT * FROM Curso WHERE Id IN (SELECT IdCurso FROM Dicta Where DniProfesor = @p0)", HttpContext.Session["Dni"]).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nro.ToString() + c.Division 

            }).OrderBy(x => x.Text);

            cursos2 = cursos.Concat(cursos2);

            ViewBag.nombreEvaluacion = db.Database.SqlQuery<string>("SELECT Titulo FROM Evaluacion WHERE Id = @p0", idEvaluacion).FirstOrDefault();
            ViewBag.nombreMateria = db.Database.SqlQuery<string>("SELECT Nombre FROM Materia WHERE Id = @p0", idMateria).FirstOrDefault();
            ViewBag.idEvaluacion = idEvaluacion;
            ViewBag.idMateria = idMateria;
            ViewBag.cursos = cursos2;
            ViewBag.cursoBuscado = "-1";
            ViewBag.añoBuscado = DateTime.Today.Year;

            alumnos.Sort();

            return View(alumnos);
        }

        [HttpPost]
        public ActionResult FiltrarCalificaciones(int idCurso, int idMateria, int idEvaluacion, int año)
        {

            List<Persona> alumnos = new List<Persona>();

            if (idCurso == -1)
            {
                alumnos = db.Database.SqlQuery<Persona>("SELECT * FROM Persona WHERE Persona.Dni IN" +
                "(SELECT DniAlumno FROM Asiste WHERE Asiste.IdCurso IN " +
                "(SELECT IdCurso FROM Dicta WHERE Dicta.IdMateria = @p0 and Dicta.DniProfesor = @p1 and Dicta.año = @p2))", idMateria, HttpContext.Session["Dni"], DateTime.Parse("01/01/" + año.ToString())).ToList();

            }
            else
            {
                alumnos = db.Database.SqlQuery<Persona>("SELECT * FROM Persona WHERE Persona.Dni IN" +
                "(SELECT DniAlumno FROM Asiste WHERE Asiste.IdCurso IN " +
                "(SELECT IdCurso FROM Dicta WHERE Dicta.IdMateria = @p0 and Dicta.DniProfesor = @p1 and Dicta.IdCurso = @p2 and Dicta.año = @p3))", idMateria, HttpContext.Session["Dni"], idCurso, DateTime.Parse("01/01/" + año.ToString())).ToList();

            }


            foreach (Persona p in alumnos)
            {
                p.Alumno = db.Alumno.Find(p.Dni);
            }



            if (alumnos == null)
            {
                alumnos = new List<Persona>();
            }


            List<SelectListItem> cursos = new List<SelectListItem>();

            cursos.Add(new SelectListItem { Value = "-1", Text = "Todos" });


            IEnumerable<SelectListItem> cursos2 = db.Curso.SqlQuery("SELECT * FROM Curso WHERE Id IN (SELECT IdCurso FROM Dicta Where DniProfesor = @p0)", HttpContext.Session["Dni"]).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nro.ToString() + c.Division

            }).OrderBy(x => x.Text);

            cursos2 = cursos.Concat(cursos2);

            ViewBag.nombreEvaluacion = db.Database.SqlQuery<string>("SELECT Titulo FROM Evaluacion WHERE Id = @p0", idEvaluacion).FirstOrDefault();
            ViewBag.nombreMateria = db.Database.SqlQuery<string>("SELECT Nombre FROM Materia WHERE Id = @p0", idMateria).FirstOrDefault();
            ViewBag.idEvaluacion = idEvaluacion;
            ViewBag.idMateria = idMateria;
            ViewBag.cursos = cursos2;
            ViewBag.cursoBuscado = idCurso;
            ViewBag.añoBuscado = año;

            alumnos.Sort();

            return View("Calificar", alumnos);
        }



        public ActionResult EditarNota(int dniAlumno, int idEvaluacion, int nota, int idMateria)
        {

            Califica cAnterior = db.Califica.Find(dniAlumno, HttpContext.Session["Dni"], idEvaluacion);

            if(cAnterior != null)
            {
                db.Califica.Remove(cAnterior);
                db.SaveChanges();
            }


            Califica c = new Califica();
            c.DniAlumno = dniAlumno;
            c.IdEvaluacion = idEvaluacion;
            c.Nota = nota;
            c.DniProfesor = (int)HttpContext.Session["Dni"];

            db.Califica.Add(c);
            db.SaveChanges();

            return RedirectToAction("Calificar", new { idMateria, idEvaluacion });
        }
    }
}
