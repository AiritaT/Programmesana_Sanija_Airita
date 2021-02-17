using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Programmesana_Sanija_Airita.Models;

namespace Programmesana_Sanija_Airita.Controllers
{
    public class FilesController : Controller
    {
        private ProgrammesanaEntities db = new ProgrammesanaEntities();

        // GET: Files
        public ActionResult Index()
        {
            var files = db.Files.Include(f => f.Category).Include(f => f.User);
            return View(files.ToList());
        }

        // GET: Files/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // GET: Files/Create
        public ActionResult Create()
        {
            ViewBag.Categories_id = new SelectList(db.Categories, "id", "Name");
            ViewBag.User_id = new SelectList(db.Users, "Username", "Name");
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Title,Description,Date,Share,Categories_id,User_id")] File file)
        {
            if (ModelState.IsValid)
            {
                file.id = Guid.NewGuid();
                db.Files.Add(file);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories_id = new SelectList(db.Categories, "id", "Name", file.Categories_id);
            ViewBag.User_id = new SelectList(db.Users, "Username", "Name", file.User_id);
            return View(file);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories_id = new SelectList(db.Categories, "id", "Name", file.Categories_id);
            ViewBag.User_id = new SelectList(db.Users, "Username", "Name", file.User_id);
            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Title,Description,Date,Share,Categories_id,User_id")] File file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories_id = new SelectList(db.Categories, "id", "Name", file.Categories_id);
            ViewBag.User_id = new SelectList(db.Users, "Username", "Name", file.User_id);
            return View(file);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
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
