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
    public class PagaController : Controller
    {
        private ColegioEntities db = new ColegioEntities();

        // GET: Paga
        public ActionResult Index()
        {
            var paga = db.Paga.Include(p => p.Alumno).Include(p => p.Cuota).OrderByDescending(x => x.FechaPago).ToList<Paga>();

            if (paga == null)
            {
                paga = new List<Paga>();
            }

            return View(paga.ToList());
        }


        [HttpPost]
        public ActionResult BuscarPagoPorDni(int? Dni)
        {
            List<Paga> pagos = new List<Paga>();


            if (Dni != null)
            {
                if (db.Alumno.Count(x => x.Dni == Dni) != 0)
                {
                    pagos = db.Paga.Where(x => x.DniAlumno == Dni).OrderByDescending(x => x.FechaPago).ToList();
                }
                else
                {
                    ViewBag.errorDni = "El alumno no existe.";
                }
            }
            else
            {
                ViewBag.errorDni = "Ingrese el D.N.I.";
            }

            return View("Index", pagos);
        }



        [HttpPost]
        public ActionResult BuscarCobroPorFecha(DateTime? desde, DateTime? hasta)
        {
            List<Paga> pagos = new List<Paga>();


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
                        pagos = db.Paga.Where(x => x.FechaPago >= desde && x.FechaPago <= hasta).ToList();
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

            return View("Index", pagos);
        }

        public ActionResult GuardarCobro(List<Paga> pagos)
        {

            foreach (Paga p in pagos)
            {
                p.Cuota = null;
                db.Paga.Add(p);
            }

            db.SaveChanges();


            return View("Index");
        }


        public ActionResult Reimprimir(int nroRecibo)
        {
            List<Paga> pagos = db.Paga.Include(x => x.Cuota).Where(x => x.NroRecibo == nroRecibo).ToList();

            float total = 0;

            foreach(Paga p in pagos)
            {
                total += (float)p.Cuota.Monto;
            }

            ViewBag.total = total;

            return View(pagos);
        }





        public ActionResult Cobro(List<Paga> pagos)
        {
            ViewBag.nroRecibo = db.Database.SqlQuery<int>("SELECT TOP 1 NroRecibo " +
                "FROM Paga " +
                "ORDER BY(NroRecibo) DESC").FirstOrDefault()+1;

            if(pagos == null)
            {
                pagos = new List<Paga>();
            }

            return View(pagos);
        }


        [HttpPost]
        public ActionResult AñadirCobro(List<Paga> pagos, int? dni, int mes, int año, int nroRecibo, string lugar, string nombreCliente)
        {
            //controlar que exista el alumno
            //controlar que esa cuota no este ya pagada
            //que no esté en la lista

            if(pagos == null)
            {
                pagos = new List<Paga>();
            }

            ViewBag.nroRecibo = nroRecibo;
            ViewBag.ultimoIngreso = dni;

            if(nombreCliente == "")
            {
                ViewBag.errorCliente = "Ingrese el nombre del cliente.";
            }

            ViewBag.nombreCliente = nombreCliente;

            DateTime fechaDelMes = new DateTime(month: mes, year: año, day: 1);

            if(dni == null)
            {
                ViewBag.errorDni = "Ingrese el D.N.I. del alumno.";

                dni = 0;
            }

            if (AlumnoServicio.Existe((int)dni))
            {
                if (db.Paga.Count(x => x.DniAlumno == dni && x.FechaDelMes == fechaDelMes) == 0)
                {

                    //controlar que la cuota existe y el monto
                    if(db.Cuota.Count<Cuota>(x => x.FechaDelMes == fechaDelMes) > 0)
                    {

                        Paga pago = new Paga();
                        pago.DniAlumno = (int)dni;
                        pago.FechaDelMes = fechaDelMes;
                        pago.NroRecibo = nroRecibo;
                        pago.Lugar = lugar;
                        pago.FechaPago = DateTime.Today;

                        pago.Cuota = db.Cuota.Find(fechaDelMes);

                        if (pagos.Contains(pago) == false)
                        {
                            pagos.Add(pago);
                        }
                        else
                        {
                            ViewBag.errorRepetido = "Ya se ha incluido esa cuota.";
                        }

                    }
                    else
                    {
                        ViewBag.errorCuotaExiste = "El mes no esta disponible para su cobro.";
                    }
                    
                }
                else
                {
                    ViewBag.errorCuotaExiste = "Ya se ha pagado esa cuota.";
                }


            }
            else
            {
                ViewBag.errorDni = "No existe un alumno con ese D.N.I.";
            }



            return View("Cobro", pagos);
        }

        public ActionResult EliminarAlumno(List<Paga> pagos, int indice, int nroRecibo)
        {

            pagos.RemoveAt(indice);

            ViewBag.nroRecibo = nroRecibo;

            return View("Cobro", pagos);
        }

        // GET: Paga/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paga paga = db.Paga.Find(id);
            if (paga == null)
            {
                return HttpNotFound();
            }
            return View(paga);
        }

        // GET: Paga/Create
        public ActionResult Create()
        {
            ViewBag.DniAlumno = new SelectList(db.Alumno, "Dni", "Dni");
            ViewBag.FechaDelMes = new SelectList(db.Cuota, "FechaDelMes", "FechaDelMes");
            return View();
        }

        // POST: Paga/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DniAlumno,FechaDelMes,NroRecibo,FechaPago,Lugar")] Paga paga)
        {
            if (ModelState.IsValid)
            {
                db.Paga.Add(paga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DniAlumno = new SelectList(db.Alumno, "Dni", "Dni", paga.DniAlumno);
            ViewBag.FechaDelMes = new SelectList(db.Cuota, "FechaDelMes", "FechaDelMes", paga.FechaDelMes);
            return View(paga);
        }

        // GET: Paga/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paga paga = db.Paga.Find(id);
            if (paga == null)
            {
                return HttpNotFound();
            }
            ViewBag.DniAlumno = new SelectList(db.Alumno, "Dni", "Dni", paga.DniAlumno);
            ViewBag.FechaDelMes = new SelectList(db.Cuota, "FechaDelMes", "FechaDelMes", paga.FechaDelMes);
            return View(paga);
        }

        // POST: Paga/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DniAlumno,FechaDelMes,NroRecibo,FechaPago,Lugar")] Paga paga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DniAlumno = new SelectList(db.Alumno, "Dni", "Dni", paga.DniAlumno);
            ViewBag.FechaDelMes = new SelectList(db.Cuota, "FechaDelMes", "FechaDelMes", paga.FechaDelMes);
            return View(paga);
        }

        // GET: Paga/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paga paga = db.Paga.Find(id);
            if (paga == null)
            {
                return HttpNotFound();
            }
            return View(paga);
        }

        // POST: Paga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paga paga = db.Paga.Find(id);
            db.Paga.Remove(paga);
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
