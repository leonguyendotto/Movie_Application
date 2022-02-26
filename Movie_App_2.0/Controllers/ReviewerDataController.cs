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

        /// <summary>
        /// Returns all Keepers in the system associated with a particular animal.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Keepers in the database taking care of a particular animal
        /// </returns>
        /// <param name="id">Animal Primary Key</param>
        /// <example>
        /// GET: api/KeeperData/ListKeepersForAnimal/1
        /// </example>
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


        /// <summary>
        /// Returns Keepers in the system not caring for a particular animal.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Keepers in the database not taking care of a particular animal
        /// </returns>
        /// <param name="id">Animal Primary Key</param>
        /// <example>
        /// GET: api/KeeperData/ListKeepersNotCaringForAnimal/1
        /// </example>
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



        /// <summary>
        /// Returns all Keepers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Keeper in the system matching up to the Keeper ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Keeper</param>
        /// <example>
        /// GET: api/KeeperData/FindKeeper/5
        /// </example>
        [ResponseType(typeof(ReviewerDto))]
        [HttpGet]
        public IHttpActionResult FindKeeper(int id)
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


        /// <summary>
        /// Updates a particular Keeper in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Keeper ID primary key</param>
        /// <param name="Keeper">JSON FORM DATA of an Keeper</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/KeeperData/UpdateKeeper/5
        /// FORM DATA: Keeper JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateReviewer(int id, Reviewer Reviewer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Reviewer.ReviewerID)
            {

                return BadRequest();
            }

            db.Entry(Reviewer).State = EntityState.Modified;

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



        /// <summary>
        /// Adds an Keeper to the system
        /// </summary>
        /// <param name="Keeper">JSON FORM DATA of an Keeper</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Keeper ID, Keeper Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/KeeperData/AddKeeper
        /// FORM DATA: Keeper JSON Object
        /// </example>
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

        /// <summary>
        /// Deletes an Keeper from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Keeper</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/KeeperData/DeleteKeeper/5
        /// FORM DATA: (empty)
        /// </example>
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