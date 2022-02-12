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

namespace Movie_App_2._0.Controllers
{
    public class MovieDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/MovieData/ListMovies
        [HttpGet]
        public IEnumerable<MovieDto> ListMovies()
        {
            List<Movie> Movies = db.Movies.ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(a => MovieDtos.Add(new MovieDto()
            {
                MovieID = a.MovieID,
                MovieTitle = a.MovieTitle,
                MovieOrigin = a.MovieOrigin,
                ReviewTitle = a.Reviews.ReviewTitle,
            }));

            return MovieDtos;
        }

        // GET: api/MovieData/FindMovie/5
        [ResponseType(typeof(Movie))]
        [HttpGet]
        public IHttpActionResult FindMovie(int id)
        {
            
            Movie Movie = db.Movies.Find(id);
            MovieDto MovieDto = new MovieDto()
            {
                MovieID = Movie.MovieID,
                MovieTitle = Movie.MovieTitle,
                MovieOrigin = Movie.MovieOrigin,
                ReviewTitle = Movie.Reviews.ReviewTitle
            };
            if (MovieDto == null)
            {
                return NotFound();
            }

            return Ok(MovieDto);
        }

        // POST: api/MovieData/UpdateMovie/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMoive(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.MovieID)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        /// Add an movie to the system 
        /// </summary>
        /// <param name="movie">Json form data of an movie</param>
        /// <returns>
        /// HEADER: 201 (CREATED) - if a create successed 
        /// CONTENT: Movie ID, Movie Data
        /// or
        /// HEADER: 400 (BAD REQUEST) - if a create failed
        /// </returns>
        /// <example>
        /// POST: api/moviedata/addmovie
        /// FORM DATA: Movie JSON Object
        /// </example>
        [ResponseType(typeof(Movie))]
        [HttpPost]
        public IHttpActionResult AddMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = movie.MovieID }, movie);
        }

        // DELETE: api/MovieData/DeleteMovie/5
        [ResponseType(typeof(Movie))]
        [HttpPost]
        public IHttpActionResult DeleteMovie(int id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
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

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.MovieID == id) > 0;
        }
    }
}