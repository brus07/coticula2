using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Coticula2.Face.Models;
using System;

namespace Coticula2.Face.Controllers
{
    public class SubmitsController : Controller
    {
        private ApplicationDbContext _context;

        public SubmitsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Submits
        public IActionResult Index()
        {
            var applicationDbContext = _context.Submits.Include(s => s.Problem).Include(s => s.ProgrammingLanguage).Include(s => s.Verdict);
            return View(applicationDbContext.ToList());
        }

        // GET: Submits/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Submit submit = _context.Submits.Include(s => s.ProgrammingLanguage).Include(s => s.Verdict).Single(m => m.SubmitID == id);
            if (submit == null)
            {
                return HttpNotFound();
            }

            return View(submit);
        }

        // GET: Submits/Create
        public IActionResult Create()
        {
            var v = this.Request.Query["problemId"];
            if (v.Count == 0)
                v = "0";
            int problemId = Int32.Parse(v.ToString());
            //ViewData["ProblemID"] = new SelectList(_context.Problems, "ProblemID", "Problem");
            ViewData["ProblemID"] = problemId;
            ViewData["ProgrammingLanguages"] = new SelectList(_context.ProgrammingLanguages, "ProgrammingLanguageID", "Name");
            return View();
        }

        // POST: Submits/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Submit submit)
        {
            if (ModelState.IsValid)
            {
                submit.SubmitTime = DateTime.Now;
                submit.VerdictId = 1;
                _context.Submits.Add(submit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["ProblemID"] = new SelectList(_context.Problems, "ProblemID", "Problem", submit.ProblemID);
            return View(submit);
        }

        // GET: Submits/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Submit submit = _context.Submits.Single(m => m.SubmitID == id);
            if (submit == null)
            {
                return HttpNotFound();
            }
            ViewData["ProblemID"] = new SelectList(_context.Problems, "ProblemID", "Problem", submit.ProblemID);
            ViewData["ProgrammingLanguages"] = new SelectList(_context.ProgrammingLanguages, "ProgrammingLanguageID", "Name", submit.ProgrammingLanguageID);
            ViewData["Verdicts"] = new SelectList(_context.Verdicts, "VerdictID", "Name", submit.VerdictId);
            return View(submit);
        }

        // POST: Submits/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Submit submit)
        {
            if (ModelState.IsValid)
            {
                _context.Update(submit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["ProblemID"] = new SelectList(_context.Problems, "ProblemID", "Problem", submit.ProblemID);
            return View(submit);
        }

        // GET: Submits/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Submit submit = _context.Submits.Single(m => m.SubmitID == id);
            if (submit == null)
            {
                return HttpNotFound();
            }

            return View(submit);
        }

        // POST: Submits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Submit submit = _context.Submits.Single(m => m.SubmitID == id);
            _context.Submits.Remove(submit);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
