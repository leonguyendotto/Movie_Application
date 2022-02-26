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
    public class ReviewDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Speciess in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Speciess in the database, including their associated species.
        /// </returns>
        /// <example>
        /// GET: api/SpeciesData/ListSpecies
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ReviewDto))]
        public IHttpActionResult ListSpecies()
        {
            List<Reviews> Reviews = db.Reviews.ToList();
            List<ReviewDto> ReviewDtos = new List<ReviewDto>();

            Reviews.ForEach(s => ReviewDtos.Add(new ReviewDto()
            {
                ReviewID = s.ReviewID,
                ReviewTitle = s.ReviewTitle,
                AlreadyPosted = s.AlreadyPosted
            }));

            return Ok(ReviewDtos);
        }

        /// <summary>
        /// Returns all Speciess in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Species in the system matching up to the Species ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Species</param>
        /// <example>
        /// GET: api/SpeciesData/FindSpecies/5
        /// </example>
        [ResponseType(typeof(ReviewDto))]
        [HttpGet]
        public IHttpActionResult FindReview(int id)
        {
            Reviews Reviews = db.Reviews.Find(id);
            ReviewDto ReviewDto = new ReviewDto()
            {
                ReviewID = Reviews.ReviewID,
                ReviewTitle = Reviews.ReviewTitle,
                AlreadyPosted = Reviews.AlreadyPosted
            };
            if (Reviews == null)
            {
                return NotFound();
            }

            return Ok(ReviewDto);
        }

        /// <summary>
        /// Updates a particular Species in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Species ID primary key</param>
        /// <param name="Species">JSON FORM DATA of an Species</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/SpeciesData/UpdateSpecies/5
        /// FORM DATA: Species JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpadteReview(int id, Reviews Reviews)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Reviews.ReviewID)
            {

                return BadRequest();
            }

            db.Entry(Reviews).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewsExists(id))
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
        /// Adds an Species to the system
        /// </summary>
        /// <param name="Species">JSON FORM DATA of an Species</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Species ID, Species Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/SpeciesData/AddSpecies
        /// FORM DATA: Species JSON Object
        /// </example>
        [ResponseType(typeof(Reviews))]
        [HttpPost]
        public IHttpActionResult AddSpecies(Reviews Reviews)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reviews.Add(Reviews);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Reviews.ReviewID }, Reviews);
        }



        // DELETE: api/ReviewData/5
        [ResponseType(typeof(Reviews))]
        [HttpPost]
        public IHttpActionResult DeleteReviews(int id)
        {
            Reviews Reviews = db.Reviews.Find(id);
            if (Reviews == null)
            {
                return NotFound();
            }

            db.Reviews.Remove(Reviews);
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

        private bool ReviewsExists(int id)
        {
            return db.Reviews.Count(e => e.ReviewID == id) > 0;
        }
    }
}