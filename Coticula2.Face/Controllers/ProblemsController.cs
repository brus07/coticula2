using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coticula2.Face.Data;
using Coticula2.Face.Models;

namespace Coticula2.Face.Controllers
{
    public class ProblemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProblemsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Problems
        public async Task<IActionResult> Index()
        {
            return View(await _context.Problems.ToListAsync());
        }

        // GET: Problems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problem = await _context.Problems.SingleOrDefaultAsync(m => m.ProblemID == id);
            if (problem == null)
            {
                return NotFound();
            }

            return View(problem);
        }

        // GET: Problems/Retest/5
        public async Task<IActionResult> Retest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submits = await _context.Submits.Where(m => m.ProblemID == id).ToListAsync();
            if (submits == null)
            {
                return NotFound();
            }

            try
            {
                foreach (var submit in submits)
                {
                    submit.VerdictId = 1;
                    submit.WorkingTime = 0;
                    submit.PeakMemoryUsed = 0;
                    _context.Update(submit);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction("Index");
        }

        // GET: Problems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Problems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProblemID,Description,Title")] Problem problem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(problem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(problem);
        }

        // GET: Problems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problem = await _context.Problems.SingleOrDefaultAsync(m => m.ProblemID == id);
            if (problem == null)
            {
                return NotFound();
            }
            return View(problem);
        }

        // POST: Problems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProblemID,Description,Title")] Problem problem)
        {
            if (id != problem.ProblemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(problem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProblemExists(problem.ProblemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(problem);
        }

        // GET: Problems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problem = await _context.Problems.SingleOrDefaultAsync(m => m.ProblemID == id);
            if (problem == null)
            {
                return NotFound();
            }

            return View(problem);
        }

        // POST: Problems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var problem = await _context.Problems.SingleOrDefaultAsync(m => m.ProblemID == id);
            _context.Problems.Remove(problem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProblemExists(int id)
        {
            return _context.Problems.Any(e => e.ProblemID == id);
        }
    }
}
