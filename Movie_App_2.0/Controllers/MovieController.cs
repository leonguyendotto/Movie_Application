using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Movie_App_2._0.Models;
using Movie_App_2._0.Models.ViewModels;
using System.Web.Script.Serialization;

namespace Movie_App_2._0.Controllers
{
    public class MovieController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static MovieController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44375/api/");
        }
        // GET: Movie/List
        public ActionResult List()
        {

            //objective: communicate with our movie data api to retrieve a list of movies
            //curl https://localhost:44375/api/moviedata/listmovies

            string url = "moviedata/listmovies";
            HttpResponseMessage response = client.GetAsync(url).Result;


            // debug
            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<MovieDto> movies = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            return View(movies);
        }

        // GET: Movie/Details/5
        public ActionResult Details(int id)
        {
            DetailsMovie ViewModel = new DetailsMovie();
            //objective: communicate with our movie data api to retrieve one movie
            //curl https://localhost:44375/api/moviedata/findmovie/{id}

            string url = "moviedata/findmovie/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // debug
            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            MovieDto SelectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;

            Debug.WriteLine("movie received:");
            Debug.WriteLine(SelectedMovie.MovieTitle);

            ViewModel.SelectedMovie = SelectedMovie;

            //show associated reviewers with this movie
            url = "reviewerdata/listreviewersformovie/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ReviewerDto> ResponsibleReviewers = response.Content.ReadAsAsync<IEnumerable<ReviewerDto>>().Result;

            ViewModel.ResponsibleReviewers = ResponsibleReviewers;

            url = "reviewerdata/listreviewersnotcaringformovie/" + id;

            response = client.GetAsync(url).Result;
            IEnumerable<ReviewerDto> AvailableReviewers = response.Content.ReadAsAsync<IEnumerable<ReviewerDto>>().Result;

            ViewModel.AvailableReviewers = AvailableReviewers;


            return View(ViewModel);
        }


        //POST: Movie/Associate/{movielid}
        [HttpPost]
        public ActionResult Associate(int id, int ReviewerID)
        {
            Debug.WriteLine("Attempting to associate movie :" + id + " with reviewer " + ReviewerID);

            //call our api to associate animal with keeper
            string url = "moviedata/associatemoviewithreviewer/" + id + "/" + ReviewerID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //Get: Movie/UnAssociate/{id}?ReviewerID={ReviewerID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int ReviewerID)
        {
            Debug.WriteLine("Attempting to unassociate movie :" + id + " with reviewer: " + ReviewerID);

            //call our api to associate movie with reviewer
            string url = "moviedata/unassociatemoviewithreviewer/" + id + "/" + ReviewerID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error ()
        {
            return View();
        }

        // GET: Movie/New
        public ActionResult New()
        {
            //information about all review in the system.
            //GET api/reviewdata/listreviews

            string url = "reviewdata/listreviews";
            HttpResponseMessage response = client.GetAsync(url).Result;
            // Which results - I can't match the response with I expected in the review object
            IEnumerable<ReviewDto> ReviewOptions = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;

            return View(ReviewOptions);
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            Debug.WriteLine("the json payload is:");
            //Debug.WriteLine(movie.MovieTitle);
            //Objective: add a new movie into our system using the API
            //Curl: https://localhost:44375/api/moviedata/addmovie
            string url = "moviedata/addmovie";

            string jsonpayload = jss.Serialize(movie);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";


            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateMovie ViewModel = new UpdateMovie();


            //the existing movie information
            string url = "moviedata/findmovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MovieDto SelectedMovie = response.Content.ReadAsAsync<MovieDto>().Result;
            ViewModel.SelectedMovie = SelectedMovie;


            //all reviews to choose from when updating this movie
            //the existing movie information
            url = "reviewdata/listreviews/";
            response = client.GetAsync(url).Result;
            IEnumerable<ReviewDto> ReviewOptions = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;

            ViewModel.ReviewOptions = ReviewOptions;

            return View(ViewModel);
        }

        // POST: Movie/Update/5
        [HttpPost]
        public ActionResult Update(int id, Movie movie)
        {
            string url = "moviedata/updatemovie/" + id;
            string jsonpayload = jss.Serialize(movie);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Movie/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "moviedata/findmovie/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MovieDto selectedmovie = response.Content.ReadAsAsync<MovieDto>().Result;
            return View(selectedmovie);
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "moviedata/deletemovie/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                // the response is not successful 
                return RedirectToAction("Error");
            }

        }
    }
}
