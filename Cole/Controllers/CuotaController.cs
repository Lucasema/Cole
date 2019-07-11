using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cole.Models;
using System.Globalization;

namespace Cole.Controllers
{
    [Filters.Autorizar(Roles = "Administrador")]
    public class CuotaController : Controller
    {
        private ColegioEntities db = new ColegioEntities();





        public ActionResult Index()
        {

            int año = DateTime.Today.Year;


            List<Cuota> cuotas = db.Cuota.Where(x => x.FechaDelMes.Year == año).ToList();

            ViewBag.año = año;

            return View(cuotas);

        }
        // GET: Cuota
        [HttpPost]
        public ActionResult BuscarAño(int año)
        {

            List<Cuota> cuotas = db.Cuota.Where(x => x.FechaDelMes.Year == año).ToList();

            ViewBag.año = año;

            return View("Index", cuotas);

        }

        // GET: Cuota/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuota cuota = db.Cuota.Find(id);
            if (cuota == null)
            {
                return HttpNotFound();
            }
            return View(cuota);
        }

        // GET: Cuota/Create
        public ActionResult Create(DateTime fechaCuota)
        {
            Cuota c = new Cuota();

            c.FechaDelMes = fechaCuota;

            ViewBag.mesCuota = fechaCuota.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));


            return View(c);
        }

        // POST: Cuota/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cuota cuota)
        {
            if (ModelState.IsValid)
            {
                db.Cuota.Add(cuota);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cuota);
        }

        // GET: Cuota/Edit/5
        public ActionResult Edit(DateTime fechaCuota)
        {
            if (fechaCuota == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuota cuota = db.Cuota.Find(fechaCuota);
            if (cuota == null)
            {
                return HttpNotFound();
            }

            ViewBag.mesCuota = fechaCuota.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));

            return View(cuota);
        }

        // POST: Cuota/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FechaDelMes,Monto")] Cuota cuota)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuota).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cuota);
        }

        // GET: Cuota/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuota cuota = db.Cuota.Find(id);
            if (cuota == null)
            {
                return HttpNotFound();
            }
            return View(cuota);
        }

        // POST: Cuota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            Cuota cuota = db.Cuota.Find(id);
            db.Cuota.Remove(cuota);
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
