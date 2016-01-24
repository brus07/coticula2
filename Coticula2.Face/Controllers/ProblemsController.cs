using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using Coticula2.Face.Models;

namespace Coticula2.Face.Controllers
{
    public class ProblemsController : Controller
    {
        private ApplicationDbContext _context;

        public ProblemsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Problems
        public IActionResult Index()
        {
            return View(_context.Problems.ToList());
        }

        // GET: Problems/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Problem problem = _context.Problems.Single(m => m.ProblemID == id);
            if (problem == null)
            {
                return HttpNotFound();
            }

            return View(problem);
        }

        // GET: Problems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Problems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Problem problem)
        {
            if (ModelState.IsValid)
            {
                _context.Problems.Add(problem);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(problem);
        }

        // GET: Problems/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Problem problem = _context.Problems.Single(m => m.ProblemID == id);
            if (problem == null)
            {
                return HttpNotFound();
            }
            return View(problem);
        }

        // POST: Problems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Problem problem)
        {
            if (ModelState.IsValid)
            {
                _context.Update(problem);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(problem);
        }

        // GET: Problems/Delete/5
        [ActionName("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Problem problem = _context.Problems.Single(m => m.ProblemID == id);
            if (problem == null)
            {
                return HttpNotFound();
            }

            return View(problem);
        }

        // POST: Problems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Problem problem = _context.Problems.Single(m => m.ProblemID == id);
            _context.Problems.Remove(problem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
