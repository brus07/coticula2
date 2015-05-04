using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Coticula2Face.Models;
using Coticula2Face.Models.Coticula;
using System.Web.Mvc;

namespace Coticula2Face.Controllers.Api
{
    public class SubmitsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // Returns ID's only untested submits
        // GET: api/Submits
        public IHttpActionResult GetSubmits()
        {
            var inQueueSubmits = db.Submits.Where(r => r.VerdictID == 1).ToArray();
            List<int> idList = new List<int>();
            foreach (var submit in inQueueSubmits)
            {
                idList.Add(submit.Id);
            }
            return Ok(idList.ToArray());
        }

        // GET: api/Submits/5
        [ResponseType(typeof(Coticula2.Connector.Submit))]
        public IHttpActionResult GetSubmit(int id)
        {
            Submit submit = db.Submits.Find(id);
            if (submit == null)
            {
                return NotFound();
            }

            Coticula2.Connector.Submit returnSubmit = new Coticula2.Connector.Submit();
            returnSubmit.Id = submit.Id;
            returnSubmit.LanguageID = submit.LanguageID;
            returnSubmit.ProblemID = submit.ProblemID;
            returnSubmit.Solution = submit.Solution;
            returnSubmit.Time = submit.Time;
            returnSubmit.VerdictID = submit.VerdictID;
            return Ok(returnSubmit);
        }

        // PUT: api/Submits/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSubmit(int id, Coticula2.Connector.ReturnSubmit returnSubmit)
        {
            var submit = db.Submits.First(r => r.Id == id);
            submit.VerdictID = returnSubmit.VerdictID;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != returnSubmit.Id)
            {
                return BadRequest();
            }

            db.Entry(submit).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubmitExists(int id)
        {
            return db.Submits.Count(e => e.Id == id) > 0;
        }
    }
}