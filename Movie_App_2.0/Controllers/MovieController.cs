using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Movie_App_2._0.Models;
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
            client.BaseAddress = new Uri("https://localhost:44375/api/moviedata/");
        }
        // GET: Movie/List
        public ActionResult List()
        {

            //objective: communicate with our animal data api to retrieve a list of movies
            //curl https://localhost:44375/api/moviedata/listmovies

            string url = "listmovies";
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

            //objective: communicate with our animal data api to retrieve one movie
            //curl https://localhost:44375/api/moviedata/findmovie/{id}

            string url = "findmovie/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // debug
            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            MovieDto selectedmovies = response.Content.ReadAsAsync<MovieDto>().Result;
            Debug.WriteLine("movie received:");
            Debug.WriteLine(selectedmovies.MovieTitle);

            return View(selectedmovies);
        }

        public ActionResult Error ()
        {
            return View();
        }

        // GET: Movie/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Movie/Create
        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            Debug.WriteLine("the json payload is:");
            //Debug.WriteLine(movie.MovieTitle);
            //Objective: add a new movie into our system using the API
            //Curl: https://localhost:44375/api/moviedata/addmovie
            string url = "addmovie";

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
            return View();
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Movie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
