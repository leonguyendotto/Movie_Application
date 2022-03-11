using System;
using System.Web;
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
using System.IO;

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
                MovieHasPic = a.MovieHasPic,
                PicExtension = a.PicExtension
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
                ReviewTitle = Movie.Reviews.ReviewTitle,
                MovieHasPic = Movie.MovieHasPic,
                PicExtension = Movie.PicExtension
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
        public IHttpActionResult UpdateMovie(int id, Movie movie)
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
            // Picture update is handled by another method
            db.Entry(movie).Property(a => a.MovieHasPic).IsModified = false;
            db.Entry(movie).Property(a => a.PicExtension).IsModified = false;

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
        /// Receives animal picture data, uploads it to the webserver and updates the animal's HasPic option
        /// </summary>
        /// <param name="id">the animal id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F animalpic=@file.jpg "https://localhost:xx/api/animaldata/uploadanimalpic/2"
        /// POST: api/animalData/UpdateanimalPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UploadMoviePoster(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var moviePoster = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (moviePoster.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(moviePoster.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/animals/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Posters/"), fn);

                                //save the file
                                moviePoster.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the movie haspic and picextension fields in the database
                                Movie Selectedmovie = db.Movies.Find(id);
                                Selectedmovie.MovieHasPic = haspic;
                                Selectedmovie.PicExtension = extension;
                                db.Entry(Selectedmovie).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Movie Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

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

            if (movie.MovieHasPic && movie.PicExtension != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/Posters/" + id + "." + movie.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
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