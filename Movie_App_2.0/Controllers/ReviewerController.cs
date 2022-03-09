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
    public class ReviewerController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ReviewerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44375/api/");
        }
        // GET: Reviewer/List
        public ActionResult List()
        {
            //objective: communicate with our Keeper data api to retrieve a list of Keepers
            //curl https://localhost:44375/api/reviewerdata/listreviewers


            string url = "reviewerdata/listreviewers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ReviewerDto> Reviewers = response.Content.ReadAsAsync<IEnumerable<ReviewerDto>>().Result;
            //Debug.WriteLine("Number of Keepers received : ");
            //Debug.WriteLine(Keepers.Count());


            return View(Reviewers);
        }

        // GET: Reviewer/Details/5
        public ActionResult Details(int id)
        {
            DetailsReviewer ViewModel = new DetailsReviewer();

            //objective: communicate with our Keeper data api to retrieve one Keeper
            //curl https://localhost:44324/api/Keeperdata/findkeeper/{id}

            string url = "reviewerdata/findreviewer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            ReviewerDto SelectedReviewer = response.Content.ReadAsAsync<ReviewerDto>().Result;
           

            ViewModel.SelectedReviewer = SelectedReviewer;

            //show all movie under the care of this reviewer
            url = "moviedata/listmoviesforreviewer/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<MovieDto> KeptMovies = response.Content.ReadAsAsync<IEnumerable<MovieDto>>().Result;

            ViewModel.KeptMovies = KeptMovies;


            return View(ViewModel);
        }


        // GET: Reviewer/New
        public ActionResult Error()
        {

            return View();
        }

        // POST: Reviewer/Create
        [HttpPost]
        public ActionResult Create(Reviewer Reviewer)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Keeper.KeeperName);
            //objective: add a new Keeper into our system using the API
            //curl -H "Content-Type:application/json" -d @Keeper.json https://localhost:44324/api/Keeperdata/addKeeper 
            string url = "reviewerdata/addreviewer";


            string jsonpayload = jss.Serialize(Reviewer);
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

        // GET: Reviewer/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "reviewerdata/findreviewer" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReviewerDto selectedReviewer = response.Content.ReadAsAsync<ReviewerDto>().Result;
            return View(selectedReviewer);
        }

        // POST: Reviewer/Update/5
        [HttpPost]
        public ActionResult Update(int id, Reviewer Keeper)
        {

            string url = "reviewerdata/updateviewer/" + id;
            string jsonpayload = jss.Serialize(Keeper);
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



        // GET: Reviewer/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "reviewerdata/findreviewer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ReviewerDto selectedReviewer = response.Content.ReadAsAsync<ReviewerDto>().Result;
            return View(selectedReviewer);
        }

        // POST: Reviewer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "reviewerdata/deletereviewer/" + id;
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
