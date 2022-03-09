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
using Movie_App_2._0.Models;
using System.Diagnostics;

namespace Movie_App_2._0.Controllers
{
    public class ReviewerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ReviewerData/ListReviewers
        [HttpGet]
        [ResponseType(typeof(ReviewerDto))]
        public IHttpActionResult ListReviewers()
        {
            List<Reviewer> Reviewers = db.Reviewers.ToList();
            List<ReviewerDto> ReviewerDtos = new List<ReviewerDto>();

            Reviewers.ForEach(k => ReviewerDtos.Add(new ReviewerDto()
            {
                ReviewerID = k.ReviewerID,
                ReviewerFirstName = k.ReviewerFirstName,
                ReviewerLastName = k.ReviewerLastName
            }));

            return Ok(ReviewerDtos);
        }

        [HttpGet]
        [ResponseType(typeof(ReviewerDto))]
        public IHttpActionResult ListReviewersForMovie(int id)
        {
            List<Reviewer> Reviewers = db.Reviewers.Where(
                k => k.Movies.Any(
                    a => a.MovieID == id)
                ).ToList();
            List<ReviewerDto> ReviewerDtos = new List<ReviewerDto>();

            Reviewers.ForEach(k => ReviewerDtos.Add(new ReviewerDto()
            {
                ReviewerID = k.ReviewerID,
                ReviewerFirstName = k.ReviewerFirstName,
                ReviewerLastName = k.ReviewerLastName
            }));

            return Ok(ReviewerDtos);
        }


    
        [HttpGet]
        [ResponseType(typeof(ReviewerDto))]
        public IHttpActionResult ListReviewersNotCaringForMovie(int id)
        {
            List<Reviewer> Reviewers = db.Reviewers.Where(
                k => !k.Movies.Any(
                    a => a.MovieID == id)
                ).ToList();
            List<ReviewerDto> ReviewerDtos = new List<ReviewerDto>();

            Reviewers.ForEach(k => ReviewerDtos.Add(new ReviewerDto()
            {
                ReviewerID = k.ReviewerID,
                ReviewerFirstName = k.ReviewerFirstName,
                ReviewerLastName = k.ReviewerLastName
            }));

            return Ok(ReviewerDtos);
        }



        [ResponseType(typeof(ReviewerDto))]
        [HttpGet]
        public IHttpActionResult FindReviewer(int id)
        {
            Reviewer Reviewer = db.Reviewers.Find(id);
            ReviewerDto ReviewerDto = new ReviewerDto()
            {
                ReviewerID = Reviewer.ReviewerID,
                ReviewerFirstName = Reviewer.ReviewerFirstName,
                ReviewerLastName = Reviewer.ReviewerLastName
            };
            if (Reviewer == null)
            {
                return NotFound();
            }

            return Ok(ReviewerDto);
        }


        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateReviewer(int id, Reviewer reviewer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reviewer.ReviewerID)
            {

                return BadRequest();
            }

            db.Entry(reviewer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewerExists(id))
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



        [ResponseType(typeof(Reviewer))]
        [HttpPost]
        public IHttpActionResult AddReviewer(Reviewer Reviewer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reviewers.Add(Reviewer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Reviewer.ReviewerID }, Reviewer);
        }

 
        [ResponseType(typeof(Reviewer))]
        [HttpPost]
        public IHttpActionResult DeleteReviewer(int id)
        {
            Reviewer Reviewer = db.Reviewers.Find(id);
            if (Reviewer == null)
            {
                return NotFound();
            }

            db.Reviewers.Remove(Reviewer);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReviewerExists(int id)
        {
            return db.Reviewers.Count(e => e.ReviewerID == id) > 0;
        }
    }
}