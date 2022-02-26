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

        /// <summary>
        /// Gathers information about all animals related to a particular species ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all animals in the database, including their associated species matched with a particular species ID
        /// </returns>
        /// <param name="id">Species ID.</param>
        /// <example>
        /// GET: api/AnimalData/ListAnimalsForSpecies/3
        /// </example>
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

        /// <summary>
        /// Gathers information about animals related to a particular keeper
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all animals in the database, including their associated species that match to a particular keeper id
        /// </returns>
        /// <param name="id">Keeper ID.</param>
        /// <example>
        /// GET: api/AnimalData/ListAnimalsForKeeper/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(MovieDto))]
        public IHttpActionResult ListAnimalsForKeeper(int id)
        {
            //all animals that have keepers which match with our ID
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

        /// <summary>
        /// Associates a particular keeper with a particular animal
        /// </summary>
        /// <param name="animalid">The animal ID primary key</param>
        /// <param name="keeperid">The keeper ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AnimalData/AssociateAnimalWithKeeper/9/1
        /// </example>
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

        /// <summary>
        /// Removes an association between a particular keeper and a particular animal
        /// </summary>
        /// <param name="animalid">The animal ID primary key</param>
        /// <param name="keeperid">The keeper ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/AnimalData/AssociateAnimalWithKeeper/9/1
        /// </example>
        [HttpPost]
        [Route("api/MovieData/UnAssociateMovieWithReviewer/{movieid}/{reviwerid}")]
        public IHttpActionResult UnAssociateAnimalWithKeeper(int animalid, int keeperid)
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