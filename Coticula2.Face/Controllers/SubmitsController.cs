using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coticula2.Face.Data;
using Coticula2.Face.Models;
using Microsoft.AspNetCore.Http;

namespace Coticula2.Face.Controllers
{
    public class SubmitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubmitsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Submits
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Submits.Include(s => s.Problem).Include(s => s.ProgrammingLanguage).Include(s => s.Verdict).OrderByDescending(s => s.SubmitID);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: /api/Product/{id}
        [Route("/api/Submits")]
        public IEnumerable<int> GetSubmits()
        {
            var untestedSubmits = _context.Submits.Where(m => m.VerdictId == 1);
            List<int> ids = new List<int>();
            foreach (var item in untestedSubmits)
            {
                ids.Add(item.SubmitID);
            }
            return ids;
        }

        // GET: Submits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submit = await _context.Submits.Include(s => s.Problem).Include(s => s.ProgrammingLanguage).Include(s => s.Verdict).SingleOrDefaultAsync(m => m.SubmitID == id);
            if (submit == null)
            {
                return NotFound();
            }

            return View(submit);
        }

        // GET: api/SubmitsApi/5
        [HttpGet("/api/Submits/{id}")]
        public async Task<IActionResult> GetSubmit([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Submit submit = await _context.Submits.SingleOrDefaultAsync(m => m.SubmitID == id);

            if (submit == null)
            {
                return NotFound();
            }

            return Ok(submit);
        }

        // GET: Submits/Retest/5
        public async Task<IActionResult> Retest(int? id)
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
            submit.VerdictId = 1;
            submit.PeakMemoryUsed = 0;
            submit.WorkingTime = 0;
            try
            {
                _context.Update(submit);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmitExists(submit.SubmitID))
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

        // GET: Submits/Create
        public IActionResult Create()
        {
            ViewData["ProblemID"] = new SelectList(_context.Problems, "ProblemID", "Title");
            ViewData["ProgrammingLanguageID"] = new SelectList(_context.ProgrammingLanguages, "ProgrammingLanguageID", "Name");
            return View();
        }

        // POST: Submits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubmitID,ProblemID,ProgrammingLanguageID,Solution")] Submit submit)
        {
            if (ModelState.IsValid)
            {
                submit.SubmitTime = DateTime.Now;
                submit.VerdictId = 1;
                _context.Add(submit);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ProblemID"] = new SelectList(_context.Problems, "ProblemID", "Title", submit.ProblemID);
            ViewData["ProgrammingLanguageID"] = new SelectList(_context.ProgrammingLanguages, "ProgrammingLanguageID", "Name", submit.ProgrammingLanguageID);
            return View(submit);
        }

        // PUT: api/SubmitsApi/5
        [HttpPut("/api/Submits/{id}")]
        public async Task<IActionResult> PutSubmit([FromRoute] int id, [FromBody] Submit submit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != submit.SubmitID)
            {
                return BadRequest();
            }

            _context.Entry(submit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmitExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: Submits/Edit/5
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
            ViewData["ProblemID"] = new SelectList(_context.Problems, "ProblemID", "Title", submit.ProblemID);
            ViewData["ProgrammingLanguageID"] = new SelectList(_context.ProgrammingLanguages, "ProgrammingLanguageID", "Name", submit.ProgrammingLanguageID);
            ViewData["VerdictId"] = new SelectList(_context.Verdicts, "VerdictID", "Name", submit.VerdictId);
            return View(submit);
        }

        // POST: Submits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubmitID,PeakMemoryUsed,ProblemID,ProgrammingLanguageID,Solution,SubmitTime,VerdictId,WorkingTime")] Submit submit)
        {
            if (id != submit.SubmitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(submit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubmitExists(submit.SubmitID))
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
            ViewData["ProblemID"] = new SelectList(_context.Problems, "ProblemID", "Title", submit.ProblemID);
            ViewData["ProgrammingLanguageID"] = new SelectList(_context.ProgrammingLanguages, "ProgrammingLanguageID", "Name", submit.ProgrammingLanguageID);
            ViewData["VerdictId"] = new SelectList(_context.Verdicts, "VerdictID", "Name", submit.VerdictId);
            return View(submit);
        }

        // POST: api/SubmitsApi
        [HttpPost("/api/Submits")]
        public async Task<IActionResult> PostSubmit([FromBody] Submit submit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Submits.Add(submit);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SubmitExists(submit.SubmitID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSubmit", new { id = submit.SubmitID }, submit);
        }

        // GET: Submits/Delete/5
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

        // POST: Submits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var submit = await _context.Submits.SingleOrDefaultAsync(m => m.SubmitID == id);
            _context.Submits.Remove(submit);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SubmitExists(int id)
        {
            return _context.Submits.Any(e => e.SubmitID == id);
        }
    }
}
