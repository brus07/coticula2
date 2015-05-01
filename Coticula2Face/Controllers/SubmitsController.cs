using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Coticula2Face.Models;
using Coticula2Face.Models.Coticula;

namespace Coticula2Face.Controllers
{
    public class SubmitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Submits
        public ActionResult Index()
        {
            var submits = db.Submits.Include(s => s.Language).Include(s => s.Problem).Include(s => s.Verdict).OrderByDescending(s=>s.Time);
            return View(submits.ToList());
        }

        // GET: Submits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submit submit = db.Submits.Find(id);
            if (submit == null)
            {
                return HttpNotFound();
            }
            return View(submit);
        }

        // GET: Submits/Create
        public ActionResult Create()
        {
            ViewBag.LanguageID = new SelectList(db.Languages, "Id", "ShortName");
            ViewBag.ProblemID = new SelectList(db.Problems, "Id", "Name");
            return View();
        }

        // POST: Submits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProblemID,LanguageID,Solution")] Submit submit)
        {
            if (ModelState.IsValid)
            {
                submit.Time = DateTime.Now;
                submit.VerdictID = 1;
                db.Submits.Add(submit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LanguageID = new SelectList(db.Languages, "Id", "ShortName", submit.LanguageID);
            ViewBag.ProblemID = new SelectList(db.Problems, "Id", "Name", submit.ProblemID);
            return View(submit);
        }

        // GET: Submits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submit submit = db.Submits.Find(id);
            if (submit == null)
            {
                return HttpNotFound();
            }
            ViewBag.LanguageID = new SelectList(db.Languages, "Id", "ShortName", submit.LanguageID);
            ViewBag.ProblemID = new SelectList(db.Problems, "Id", "Name", submit.ProblemID);
            ViewBag.VerdictID = new SelectList(db.Verdicts, "Id", "Name", submit.VerdictID);
            return View(submit);
        }

        // POST: Submits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VerdictID")] Submit submit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LanguageID = new SelectList(db.Languages, "Id", "ShortName", submit.LanguageID);
            ViewBag.ProblemID = new SelectList(db.Problems, "Id", "Name", submit.ProblemID);
            ViewBag.VerdictID = new SelectList(db.Verdicts, "Id", "Name", submit.VerdictID);
            return View(submit);
        }

        // GET: Submits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submit submit = db.Submits.Find(id);
            if (submit == null)
            {
                return HttpNotFound();
            }
            return View(submit);
        }

        // POST: Submits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Submit submit = db.Submits.Find(id);
            db.Submits.Remove(submit);
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
