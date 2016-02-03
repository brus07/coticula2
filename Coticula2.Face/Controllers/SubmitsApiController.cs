using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Coticula2.Face.Models;

namespace Coticula2.Face.Controllers
{
    [Produces("application/json")]
    [Route("api/SubmitsApi")]
    public class SubmitsApiController : Controller
    {
        private ApplicationDbContext _context;

        public SubmitsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SubmitsApi
        [HttpGet]
        public IEnumerable<int> GetSubmits()
        {
            var untestedSubmits = _context.Submits.Where(m => m.Status == 0);
            List<int> ids = new List<int>();
            foreach (var item in untestedSubmits)
            {
                ids.Add(item.SubmitID);
            }
            return ids;
            //return _context.Submits;
        }

        // GET: api/SubmitsApi/5
        [HttpGet("{id}", Name = "GetSubmit")]
        public IActionResult GetSubmit([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            Submit submit = _context.Submits.Single(m => m.SubmitID == id);

            if (submit == null)
            {
                return HttpNotFound();
            }

            return Ok(submit);
        }

        // PUT: api/SubmitsApi/5
        [HttpPut("{id}")]
        public IActionResult PutSubmit(int id, [FromBody] Submit submit)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            if (id != submit.SubmitID)
            {
                return HttpBadRequest();
            }
            _context.Submits.Single(m => m.SubmitID == id).Status = submit.Status;
            //_context.Entry(submit).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubmitExists(id))
                {
                    return HttpNotFound();
                }
                else
                {
                    throw;
                }
            }

            return new HttpStatusCodeResult(StatusCodes.Status204NoContent);
        }

        // POST: api/SubmitsApi
        [HttpPost]
        public IActionResult PostSubmit([FromBody] Submit submit)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            _context.Submits.Add(submit);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SubmitExists(submit.SubmitID))
                {
                    return new HttpStatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetSubmit", new { id = submit.SubmitID }, submit);
        }

        // DELETE: api/SubmitsApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteSubmit(int id)
        {
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            Submit submit = _context.Submits.Single(m => m.SubmitID == id);
            if (submit == null)
            {
                return HttpNotFound();
            }

            _context.Submits.Remove(submit);
            _context.SaveChanges();

            return Ok(submit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubmitExists(int id)
        {
            return _context.Submits.Count(e => e.SubmitID == id) > 0;
        }
    }
}