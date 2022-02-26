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
            return View();
        }

        // GET: Reviewer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reviewer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reviewer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Reviewer/Edit/5
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

        // GET: Reviewer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Reviewer/Delete/5
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
