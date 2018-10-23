using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PapelesApp.Models;

namespace PapelesApp.Controllers
{
    public class MaestroPapelesController : Controller
    {
        private Entities db = new Entities();

        // GET: IPP_MAESTRO_PAPELES


        public ActionResult Index(String busqueda)
        {

            var iPP_MAESTRO_PAPELES = db.IPP_MAESTRO_PAPELES.Include(i => i.IPP_TIPOS_DE_PAPEL);
            var codigo = from s in db.IPP_MAESTRO_PAPELES select s;
            if (!String.IsNullOrEmpty(busqueda))
            {
                codigo = codigo.Where(j => j.MP_CODIGO_ALTERNO.Contains(busqueda));
            }
            return View(codigo.ToList());
        }

        // GET: IPP_MAESTRO_PAPELES_BK/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IPP_MAESTRO_PAPELES iPP_MAESTRO_PAPELES = db.IPP_MAESTRO_PAPELES.Find(id);
            if (iPP_MAESTRO_PAPELES == null)
            {
                return HttpNotFound();
            }
            return View(iPP_MAESTRO_PAPELES);
        }

        // GET: IPP_MAESTRO_PAPELES_BK/Create
        public ActionResult Create()
        {
            ViewBag.MP_ID_TIPO_PAPEL = new SelectList(db.IPP_TIPOS_DE_PAPEL, "TDP_CODIGO", "TDP_DESCRIPCION");
            
            return View();

        }

        // POST: IPP_MAESTRO_PAPELES_BK/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MP_ID_PAPEL,MP_ID_TIPO_PAPEL,MP_CALIBRE,MP_ANCHO,MP_CODIGO_ALTERNO")] IPP_MAESTRO_PAPELES iPP_MAESTRO_PAPELES)
        {
            if (ModelState.IsValid)
            {

                var tipo = db.IPP_TIPOS_DE_PAPEL.Find(iPP_MAESTRO_PAPELES.MP_ID_TIPO_PAPEL);
                iPP_MAESTRO_PAPELES.MP_ID_PAPEL = 0;
                iPP_MAESTRO_PAPELES.MP_CODIGO_ALTERNO = string.Format("{0}{1}{2}", tipo.TDP_CLAVE, iPP_MAESTRO_PAPELES.MP_CALIBRE, iPP_MAESTRO_PAPELES.MP_ANCHO);
                db.IPP_MAESTRO_PAPELES.Add(iPP_MAESTRO_PAPELES);
                db.SaveChanges();
                TempData["notice"] = "Se realizo el registro correctamente";
                return RedirectToAction("Index");
            }
            ViewBag.MP_ID_TIPO_PAPEL = new SelectList(db.IPP_TIPOS_DE_PAPEL, "TDP_CODIGO", "TDP_DESCRIPCION", iPP_MAESTRO_PAPELES.MP_ID_TIPO_PAPEL);
            return View(iPP_MAESTRO_PAPELES);
        }

        // GET: IPP_MAESTRO_PAPELES_BK/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IPP_MAESTRO_PAPELES iPP_MAESTRO_PAPELES = db.IPP_MAESTRO_PAPELES.Find(id);
            if (iPP_MAESTRO_PAPELES == null)
            {
                return HttpNotFound();
            }
            ViewBag.MP_ID_TIPO_PAPEL = new SelectList(db.IPP_TIPOS_DE_PAPEL, "TDP_CODIGO", "TDP_DESCRIPCION", iPP_MAESTRO_PAPELES.MP_ID_TIPO_PAPEL);
            return View(iPP_MAESTRO_PAPELES);
        }

        // POST: IPP_MAESTRO_PAPELES_BK/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MP_ID_PAPEL,MP_ID_TIPO_PAPEL,MP_CALIBRE,MP_ANCHO,MP_CODIGO_ALTERNO")] IPP_MAESTRO_PAPELES iPP_MAESTRO_PAPELES)
        {
            if (ModelState.IsValid)
            {
                var tipo = db.IPP_TIPOS_DE_PAPEL.Find(iPP_MAESTRO_PAPELES.MP_ID_TIPO_PAPEL);
                db.Entry(iPP_MAESTRO_PAPELES).State = EntityState.Modified;
                db.SaveChanges();
                TempData["noticia"] = "Se editó el registro correctamente";
                return RedirectToAction("Index");


            }
            ViewBag.MP_ID_TIPO_PAPEL = new SelectList(db.IPP_TIPOS_DE_PAPEL, "TDP_CODIGO", "TDP_DESCRIPCION", iPP_MAESTRO_PAPELES.MP_ID_TIPO_PAPEL);
            return View(iPP_MAESTRO_PAPELES);
        }

        // GET: IPP_MAESTRO_PAPELES_BK/Delete/5
        //public ActionResult Delete(decimal id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    IPP_MAESTRO_PAPELES iPP_MAESTRO_PAPELES = db.IPP_MAESTRO_PAPELES.Find(id);
        //    if (iPP_MAESTRO_PAPELES == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(iPP_MAESTRO_PAPELES);
        //}

        //// POST: IPP_MAESTRO_PAPELES_BK/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(decimal id)
        //{
        //    IPP_MAESTRO_PAPELES iPP_MAESTRO_PAPELES = db.IPP_MAESTRO_PAPELES.Find(id);
        //    db.IPP_MAESTRO_PAPELES.Remove(iPP_MAESTRO_PAPELES);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
