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
    public class MovieDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/MovieData/ListMovies
        [HttpGet]
        [ResponseType(typeof(MovieDto))]
        public IHttpActionResult ListMovies()
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

            return Ok(MovieDtos);
        }

    
        [HttpGet]
        [ResponseType(typeof(MovieDto))]
        public IHttpActionResult ListMoviesForReviews(int id)
        {
            List<Movie> Movies = db.Movies.Where(a => a.ReviewID == id).ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(a => MovieDtos.Add(new MovieDto()
            {
                MovieID = a.MovieID,
                MovieTitle = a.MovieTitle,
                MovieOrigin = a.MovieOrigin,
                ReviewID = a.Reviews.ReviewID,
                ReviewTitle = a.Reviews.ReviewTitle
            }));

            return Ok(MovieDtos);
        }

        
        [HttpGet]
        [ResponseType(typeof(MovieDto))]
        public IHttpActionResult ListMoviesForReviewer(int id)
        {
          
            List<Movie> Movies = db.Movies.Where(
                a => a.Reviewers.Any(
                    k => k.ReviewerID == id
                )).ToList();
            List<MovieDto> MovieDtos = new List<MovieDto>();

            Movies.ForEach(a => MovieDtos.Add(new MovieDto()
            {
                MovieID = a.MovieID,
                MovieTitle = a.MovieTitle,
                MovieOrigin = a.MovieOrigin,
                ReviewID = a.Reviews.ReviewID,
                ReviewTitle = a.Reviews.ReviewTitle
            }));

            return Ok(MovieDtos);
        }

        [HttpPost]
        [Route("api/MovieData/AssociateMovieWithReviewer/{movieid}/{reviewerid}")]
        public IHttpActionResult AssociateMovieWithReviewer(int movieid, int reviewerid)
        {

            Movie SelectedMovie = db.Movies.Include(a => a.Reviewers).Where(a => a.MovieID == movieid).FirstOrDefault();
            Reviewer SelectedReviewer = db.Reviewers.Find(reviewerid);

            if (SelectedMovie == null || SelectedReviewer == null)
            {
                return NotFound();
            }

            SelectedMovie.Reviewers.Add(SelectedReviewer);
            db.SaveChanges();

            return Ok();
        }

     
        [HttpPost]
        [Route("api/MovieData/UnAssociateMovieWithReviewer/{movieid}/{reviewerid}")]
        public IHttpActionResult UnAssociateMovieWithReviewer(int movieid, int reviewerid)
        {

            Movie SelectedMovie = db.Movies.Include(a => a.Reviewers).Where(a => a.MovieID == movieid).FirstOrDefault();
            Reviewer SelectedReviewer = db.Reviewers.Find(reviewerid);

            if (SelectedMovie == null || SelectedReviewer == null)
            {
                return NotFound();
            }

            SelectedMovie.Reviewers.Remove(SelectedReviewer);
            db.SaveChanges();

            return Ok();
        }



        // GET: api/MovieData/FindMovie/5
        [ResponseType(typeof(MovieDto))]
        [HttpGet]
        public IHttpActionResult FindMovie(int id)
        {
            
            Movie Movie = db.Movies.Find(id);
            MovieDto MovieDto = new MovieDto()
            {
                MovieID = Movie.MovieID,
                MovieTitle = Movie.MovieTitle,
                MovieOrigin = Movie.MovieOrigin,
                ReviewID = Movie.Reviews.ReviewID,
                ReviewTitle = Movie.Reviews.ReviewTitle
            };
            if (Movie == null)
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