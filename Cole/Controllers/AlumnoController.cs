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
    [Filters.Autorizar(Roles = "Administrador")]
    public class AlumnoController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Alumno
        public ActionResult Index()
        {
            var alumno = db.Alumno.Include(a => a.Persona).Include(a => a.Tutor);
            
            return View(alumno.ToList());
        }

        [HttpPost]
        public ActionResult Index(string campo, string valor)
        {
            if (valor != "")
            {
                List<Alumno> alumnos = AlumnoServicio.Buscar(campo, valor);
                return View(alumnos);
            }
            return Index();
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
                
                alumno.Dni = alumno.Persona.Dni;

                alumno.DniTutor = alumno.Tutor.Dni;

                alumno.Persona.Contraseña = alumno.Dni.ToString();

                Persona p = new Persona();
                p.Dni = alumno.Tutor.Dni;
                p.Apellido = "-";
                p.Nombre = "-";
                p.FechaNacimiento = DateTime.MinValue;
                p.Cuil = "-";
                p.Domicilio = "-";
                p.Nacionalidad = "-";
                p.Sexo = "-";
                p.TelCelular = 0;
                p.TelFijo = 0;
                p.Contraseña = alumno.DniTutor.ToString();

                alumno.Tutor.Persona = p;



                try
                {
                    db.Alumno.Add(alumno);
                    db.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    Persona result = db.Database.SqlQuery<Persona>("SELECT * FROM Persona WHERE Persona.Dni = @p0", parameters: alumno.Dni).FirstOrDefault();

                    if ( result != null)
                    {
                        ViewBag.keyDuplicadaAlumno = "Ya existe una persona con ese D.N.I.";
                        return View(alumno);
                    }

                    Tutor t = db.Tutor.Find(alumno.DniTutor);

                    if (t != null)
                    {
                        object[] parametros = { alumno.Persona.Dni, alumno.Persona.Cuil, alumno.Persona.TelCelular, alumno.Persona.TelFijo, alumno.Persona.Dni.ToString(), alumno.Persona.Sexo, alumno.Persona.Domicilio, alumno.Persona.Nacionalidad, alumno.Persona.Nombre, alumno.Persona.Apellido, alumno.Persona.FechaNacimiento };

                        db.Database.ExecuteSqlCommand("INSERT INTO Persona (Dni, Cuil, TelCelular, TelFijo, Contraseña, Sexo, Domicilio, Nacionalidad, Nombre, Apellido, FechaNacimiento)" +
                            " VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)", alumno.Persona.Dni.ToString(), alumno.Persona.Cuil, alumno.Persona.TelCelular, alumno.Persona.TelFijo, alumno.Persona.Dni.ToString(), alumno.Persona.Sexo, alumno.Persona.Domicilio, alumno.Persona.Nacionalidad, alumno.Persona.Nombre, alumno.Persona.Apellido, alumno.Persona.FechaNacimiento);

                        db.Database.ExecuteSqlCommand("INSERT INTO Alumno (Dni, DniTutor)" +
                            " VALUES (@p0, @p1)", alumno.Dni, alumno.DniTutor);

                        return Index();
                    }

                    throw e;
                }

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

            return View(alumno);
        }

        // POST: Alumno/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Alumno alumno)
        {
            alumno.Dni = alumno.Persona.Dni;
            alumno.DniTutor = alumno.Tutor.Dni;
            
            //hay que tener en cuenta que el tutor se puede cambiar!



            if (ModelState.IsValid)
            {
                db.Entry(alumno).State = EntityState.Modified;
                db.Entry(alumno.Persona).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            Persona p = db.Persona.Find(id);

            db.Alumno.Remove(alumno);
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
