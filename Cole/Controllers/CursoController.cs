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
                //comprueba que no exista un curso con ese nro y division
                if(!CursoServicio.Existe(curso.Nro, curso.Division))
                {

                    db.Curso.Add(curso);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }

                //si ya existe un curso con ese nro y division, se lo muestra por mensaje
                ViewBag.KeyCursoDuplicado = "Ya existe un curso con ese número y division.";
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

            
            if (curso == null)
            {
                return HttpNotFound();
            }
            return View(curso);
        }



        public ActionResult EditarAlumnosPorCurso(int idCurso, int? año)
        {

            if (año != null)
            {
                List<Persona> AlumnosDelCurso = db.Database.SqlQuery<Persona>(
                "SELECT * " +
                "FROM Persona " +
                "WHERE Persona.Dni IN " +
                "(SELECT DniAlumno FROM Asiste WHERE Asiste.IdCurso = @p0 AND Asiste.Año = @p1)", idCurso.ToString(), DateTime.Parse("01/01/"+año.ToString())
                ).ToList<Persona>();

                ViewBag.idCurso = idCurso;

                ViewBag.viendoAño = DateTime.Parse("01/01/" + año.ToString()).Year;

                return View("EditarAlumnosPorCurso", AlumnosDelCurso);

            }
            else
            {
                ViewBag.ErrorBuscarAño = "Ingrese un año.";
            }

            return EditarAlumnosPorCurso(idCurso, Int32.Parse(DateTime.Now.Year.ToString()));
        }







        [HttpPost]
        public ActionResult AñadirAlumnoAlCurso(int? dni, int idCurso, int? año)
        {
            
            if (dni != null && año!=null )
            {
                int dni1 = (int)dni;
                int año1 = (int)año;

                if (AlumnoServicio.Existe(dni1))
                {

                    if (!CursoServicio.Asiste(dni1, idCurso, año1))
                    {

                        Asiste a = new Asiste();

                        a.año = DateTime.Parse("01/01/" + año.ToString());
                        a.DniAlumno = dni1;
                        a.IdCurso = idCurso;


                        db.Asiste.Add(a);
                        db.SaveChanges();

                    }
                    else
                    {
                        ViewBag.ErrorAñadirAlumno = "Ya existe este alumno en el curso.";
                    }


                }
                else
                {
                    ViewBag.ErrorAñadirAlumno = "El alumno no existe.";
                }

                return EditarAlumnosPorCurso(idCurso, año);
            }
            else
            {
                ViewBag.ErrorAñadirAlumno = "Complete los campos.";
            }
            

            return EditarAlumnosPorCurso(idCurso, Int32.Parse(DateTime.Now.Year.ToString()) );

        }

        [HttpPost]
        public ActionResult EliminarAlumno(int dni, int idCurso, int año)
        {
            if (AlumnoServicio.Existe(dni))
            {

                Asiste a = db.Asiste.Find(dni, DateTime.Parse("01/01/" + año.ToString()), idCurso);
                db.Asiste.Remove(a);
                db.SaveChanges();

            }

            return EditarAlumnosPorCurso(idCurso, año);

        }


        // POST: Curso/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Curso curso)
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



        public ActionResult TomarAsistencia(int idCurso)
        {

            List<Persona> alumnos = db.Database.SqlQuery<Persona>("SELECT * " +
                "FROM Persona " +
                "WHERE Persona.Dni IN " +
                "(SELECT DniAlumno FROM Asiste WHERE Asiste.IdCurso= @p0 AND Asiste.año = @p1)", idCurso, "01/01/"+DateTime.Now.Year.ToString()).ToList();

            return View(alumnos);
        }


        [HttpPost]
        public ActionResult AñadirInasistencias(List<int> dnisFaltaCompleta, List<int> dnisMediaFalta)
        {
            List<int> faltaymedia = dnisFaltaCompleta.Intersect(dnisMediaFalta).ToList();

            List<int> faltaCompleta = dnisFaltaCompleta.Except(faltaymedia).ToList().Except(dnisMediaFalta).ToList();

            List<int> mediaFalta = dnisMediaFalta.Except(faltaymedia).ToList().Except(dnisFaltaCompleta).ToList();


            foreach(int dni in dnisFaltaCompleta)
            {
                Inasistencia i = new Inasistencia();

            }


            return Index();
        }

    }
}
