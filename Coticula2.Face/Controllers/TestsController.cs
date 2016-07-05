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
    public class TestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Tests
        public async Task<IActionResult> Index(int? problemId)
        {
            if (problemId == null)
            {
                return NotFound();
            }

            var applicationDbContext = _context.Submits.Include(s => s.Problem).Include(s => s.ProgrammingLanguage).Include(s => s.SubmitType).Include(s => s.Verdict).Where(s => s.SubmitTypeId == SubmitType.Test).Where(s => s.ProblemID == problemId);
            ViewData["ProblemId"] = (int)problemId;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submit = await _context.Submits.SingleOrDefaultAsync(m => m.SubmitID == id);
            if (submit == null)
            {
                return NotFound();
            }

            return View(submit);
        }

        // GET: Tests/Create
        public IActionResult Create(int problemId)
        {
            if (!ProblemExists(problemId))
            {
                return NotFound();
            }
            ViewData["Problems"] = new SelectList(_context.Problems, "ProblemID", "Title", problemId);
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubmitID,PeakMemoryUsed,ProblemID,ProgrammingLanguageID,Solution,SubmitTime,SubmitTypeId,VerdictId,WorkingTime")] Submit submit)
        {
            if (ModelState.IsValid)
            {
                submit.SubmitTime = DateTime.Now;
                submit.VerdictId = Verdict.Waiting;
                submit.ProgrammingLanguageID = ProgrammingLanguage.Text;
                submit.SubmitTypeId = SubmitType.Test;
                _context.Add(submit);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { problemId = submit.ProblemID });
            }
            ViewData["Problems"] = new SelectList(_context.Problems, "ProblemID", "Title", submit.ProblemID);
            return View(submit);
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submit = await _context.Submits.SingleOrDefaultAsync(m => m.SubmitID == id);
            if (submit == null)
            {
                return NotFound();
            }
            ViewData["VerdictId"] = new SelectList(_context.Verdicts, "VerdictID", "Name", submit.VerdictId);
            return View(submit);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubmitID,PeakMemoryUsed,ProblemID,ProgrammingLanguageID,Solution,SubmitTime,SubmitTypeId,VerdictId,WorkingTime")] Submit submit)
        {
            if (id != submit.SubmitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var submitLocal = await _context.Submits.SingleOrDefaultAsync(m => m.SubmitID == id);
                if (submitLocal == null)
                {
                    return NotFound();
                }
                submitLocal.Solution = submit.Solution;
                submitLocal.VerdictId = submit.VerdictId;
                submitLocal.SubmitTime = submit.SubmitTime;

                try
                {
                    _context.Update(submitLocal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubmitExists(submitLocal.SubmitID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { problemId = submit.ProblemID });
            }
            ViewData["VerdictId"] = new SelectList(_context.Verdicts, "VerdictID", "VerdictID", submit.VerdictId);
            return View(submit);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submit = await _context.Submits.SingleOrDefaultAsync(m => m.SubmitID == id);
            if (submit == null)
            {
                return NotFound();
            }

            return View(submit);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var submit = await _context.Submits.SingleOrDefaultAsync(m => m.SubmitID == id);
            _context.Submits.Remove(submit);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProblemExists(int id)
        {
            return _context.Problems.Any(e => e.ProblemID == id);
        }

        private bool SubmitExists(int id)
        {
            return _context.Submits.Any(e => e.SubmitID == id);
        }
    }
}
