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
using Cole.Servicios;

namespace Cole.Controllers
{
    
    public class ProfesorsController : Controller
    {
        private ColegioEntities db = new ColegioEntities();
        [Filters.Autorizar(Roles = "Administrador")]
        // GET: Profesors
        public ActionResult Index()
        {
            var profesor = db.Profesor.Include(p => p.Persona);
            return View(profesor.ToList());
        }

        [Filters.Autorizar(Roles = "Administrador")]
        public ActionResult AsignarMaterias(int? Dni)
        {
           
            List<Dicta> dicta = db.Dicta.Include(x => x.Materia).Include(x => x.Curso).Where(x => x.DniProfesor == Dni && x.año.Year == DateTime.Today.Year).ToList();

            CargarVistaDictados((int)Dni);

            return View(dicta);
        }

        

        public void CargarVistaDictados(int Dni)
        {
             Persona profe = db.Persona.Find(Dni);

            ViewBag.NomYapellido = profe.Nombre + " " + profe.Apellido;
            ViewBag.dni = Dni;

            IEnumerable<SelectListItem> materias = db.Materia.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre.ToString()

            }).OrderBy(x => x.Text);

            ViewBag.materias = materias;

            IEnumerable<SelectListItem> cursos = db.Curso.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nro.ToString() + " " + c.Division

            }).OrderBy(x => x.Text);

            ViewBag.cursos = cursos;

        }


        [Filters.Autorizar(Roles = "Administrador")]
        [HttpPost]
        public ActionResult AñadirDictado(int materia, int curso, int Dni)
        {
            //ya existe el dictado

            Dicta d = new Dicta();

            d.IdMateria = materia;
            d.IdCurso = curso;
            d.DniProfesor = Dni;
            d.año = DateTime.Parse("01/01/" + DateTime.Today.Year);

            if(db.Dicta.Find(d.IdMateria, d.año, d.IdCurso) == null)
            {
                db.Dicta.Add(d);
                db.SaveChanges();
            }
            else
            {
                ViewBag.errorExiste = "Esa materia ya está asignada a un profesor.";
            }

            CargarVistaDictados(Dni);

            List<Dicta> dicta = db.Dicta.Include(x => x.Materia).Include(x => x.Curso).Where(x => x.DniProfesor == Dni && x.año.Year == DateTime.Today.Year).ToList();

            return View("AsignarMaterias", dicta);
        }
        [Filters.Autorizar(Roles = "Administrador")]
        [HttpPost]
        public ActionResult EliminarDictado(int materia, int curso, int Dni)
        {

            Dicta d = db.Dicta.Find(materia, DateTime.Parse("01/01/" + DateTime.Today.Year), curso);

            db.Dicta.Remove(d);
            db.SaveChanges();

            CargarVistaDictados(Dni);

            List<Dicta> dicta = db.Dicta.Include(x => x.Materia).Include(x => x.Curso).Where(x => x.DniProfesor == Dni && x.año.Year == DateTime.Today.Year).ToList();

            return View("AsignarMaterias", dicta);

        }



        [Filters.Autorizar(Roles = "Administrador")]
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
        [Filters.Autorizar(Roles = "Administrador")]
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
        [Filters.Autorizar(Roles = "Administrador")]
        // GET: Profesors/Create
        public ActionResult Create()
        {
            return View();
        }
        [Filters.Autorizar(Roles = "Administrador")]
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
                try
                {
                    db.SaveChanges();
                }catch(DbUpdateException e)
                {
                    ViewBag.KeyDuplicada = "Ya existe una persona con ese D.N.I.";
                    return View(profesor);
                }
                return RedirectToAction("Index");
            }

            ViewBag.Dni = new SelectList(db.Persona, "Dni", "Cuil", profesor.Dni);
            return View(profesor);
        }
        [Filters.Autorizar(Roles = "Administrador")]
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
        [Filters.Autorizar(Roles = "Administrador")]
        // POST: Profesors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Profesor profesor)
        {
            profesor.Dni = profesor.Persona.Dni;

            List<Titulo> borrar = db.Titulo.Where(x => x.Dni == profesor.Dni).ToList();

            foreach(Titulo t in borrar)
            {
                db.Titulo.Remove(t);
            }

            foreach (Titulo t in profesor.Titulo)
            {

                t.Dni = profesor.Dni;


                if (db.Titulo.Find(t.Dni, t.Nombre) == null)
                {
                    db.Titulo.Add(t);
                }
            }

            db.SaveChanges();
            profesor.Titulo = null;

            if (ModelState.IsValid)
            {
                db.Entry(profesor).State = EntityState.Modified;
                db.Entry(profesor.Persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profesor);
        }
        [Filters.Autorizar(Roles = "Administrador")]
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
        [Filters.Autorizar(Roles = "Administrador")]
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

        [Filters.Autorizar(Roles = "Profesor")]
        public ActionResult ManualProfesor()
        {


            return View();
        }
    }
}
