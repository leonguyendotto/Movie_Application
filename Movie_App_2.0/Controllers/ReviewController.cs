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
    public class ReviewController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ReviewController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44375/api/");
        }

        //GET: Review/List
        public ActionResult List()
        {
   
            string url = "reviewdata/listreviews";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ReviewDto> Review = response.Content.ReadAsAsync<IEnumerable<ReviewDto>>().Result;
         


            return View(Review);
        }

        // GET: Review/Details/5
        public ActionResult Details(int id)
        {
           
            DetailsReview ViewModel = new DetailsReview();

            string url = "reviewdata/findreview/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            ReviewDto SelectedReview = response.Content.ReadAsAsync<ReviewDto>().Result;
           

            ViewModel.SelectedReview = SelectedReview;
          
            url = "moviedata/listmoviesforreviews/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MovieDto> RelatedMovie = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            ViewModel.RelatedMovie = RelatedMovie;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Review/New
        public ActionResult New()
        {
            return View();
        }
        // POST: Review/Create
        [HttpPost]
        public ActionResult Create(Reviews Review)
        {
            Debug.WriteLine("the json payload is :");
            string url = "reviewdata/addreview";


            string jsonpayload = jss.Serialize(Review);
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

        // GET: Review/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "reviewdata/findreview/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReviewDto selectedReview = response.Content.ReadAsAsync<ReviewDto>().Result;
            return View(selectedReview);
        }

        // POST: Review/Update/5
        [HttpPost]
        public ActionResult Update(int id, Reviews Reviews)
        {

            string url = "reviewdata/updatereview/" + id;
            string jsonpayload = jss.Serialize(Reviews);
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

        // GET: Review/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "reviewdata/findreview/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReviewDto selectedReview = response.Content.ReadAsAsync<ReviewDto>().Result;
            return View(selectedReview);
        }
        // POST: Review/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "reviewdata/deletereview/" + id;
            HttpContent content = new StringContent("");
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
    }
}
