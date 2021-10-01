using FlowerStore.ProjModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FlowerClient.Controllers
{
    public class FlowerController : Controller
    {
        string Baseurl = "https://localhost:44318/";

        public async Task<ActionResult> GetAllFlowers()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            List<Flower> FlowerInfo = new List<Flower>();
            ViewData["Useradmin"] = "False"; //Get Data for user or admin from session
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Flower/GetAllFlower");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    FlowerInfo = JsonConvert.DeserializeObject<List<Flower>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(FlowerInfo);
            }
        }

        public async Task<ActionResult> AddFlower()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            List<Occasion> OccInfo = new List<Occasion>();

            using (var client1 = new HttpClient())
            {
                //Passing service base url  
                client1.BaseAddress = new Uri(Baseurl);

                client1.DefaultRequestHeaders.Clear();
                //Define request data format  
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res1 = await client1.GetAsync("api/Flower/GetAllOccasion");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    OccInfo = JsonConvert.DeserializeObject<List<Occasion>>(EmpResponse);

                }
            }

            ViewBag.occList = OccInfo;

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> AddFlower(Flower f)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            Flower Flobj = new Flower();

            f.Occassion = Request.Form["category"];

            string fileName = Request.Form["floImg"];

            string filePath = "wwwroot/image/flowerimages/" + fileName;
            f.FlImage = System.IO.File.ReadAllBytes(filePath);

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(f), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("api/Flower/RegisterFlower", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Flobj = JsonConvert.DeserializeObject<Flower>(apiResponse);
                }
            }
            return RedirectToAction("Home", "Sitehome");            //file and folder
        }

        [HttpGet]
        public async Task<ActionResult> UpdateFlower(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            List<Occasion> OccInfo = new List<Occasion>();

            Flower fl = new Flower();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                using (var response = await httpClient.GetAsync("api/Flower/FlowerbyId?id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    fl = JsonConvert.DeserializeObject<Flower>(apiResponse);
                }
            }

            using (var client1 = new HttpClient())
            {
                //Passing service base url  
                client1.BaseAddress = new Uri(Baseurl);

                client1.DefaultRequestHeaders.Clear();
                //Define request data format  
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res1 = await client1.GetAsync("api/Flower/GetAllOccasion");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res1.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    OccInfo = JsonConvert.DeserializeObject<List<Occasion>>(EmpResponse);

                }
            }

            ViewBag.occList = OccInfo;

            return View(fl);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFlower(Flower f)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            Flower receivedflower = new Flower();

            f.Occassion = Request.Form["category"];

            string fileName = Request.Form["floImg"];

            string filePath = "wwwroot/image/flowerimages/" + fileName;
            f.FlImage = System.IO.File.ReadAllBytes(filePath);

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                int id = f.Id;
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(f), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("api/Flower/UpdateFlower", content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    receivedflower = JsonConvert.DeserializeObject<Flower>(apiResponse);
                }
            }
            return RedirectToAction("Home", "Sitehome");            //file and folder
        }

        [HttpGet]
        public async Task<ActionResult> DeleteFlower(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            Flower fl = new Flower();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                using (var response = await httpClient.GetAsync("api/Flower/FlowerbyId?id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    fl = JsonConvert.DeserializeObject<Flower>(apiResponse);
                }
            }
            return View(fl);
        }


        [HttpPost]
        // [ActionName("DeleteEmployee")]
        public async Task<ActionResult> DeleteFlowerNow(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            //int empid = Convert.ToInt32(TempData["empid"]);
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                
                using (var response = await httpClient.DeleteAsync("api/Flower/DeleteFlowerbyId?id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("Home", "Sitehome");            //file and folder
        }

        [HttpGet]
        public async Task<ActionResult> AddToCart(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            var customid = HttpContext.Session.GetInt32("Userid");
            
            Flower f1 = new Flower();
            Cart c1 = new Cart();

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                using (var response = await httpClient.GetAsync("api/Flower/FlowerbyId?id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    f1 = JsonConvert.DeserializeObject<Flower>(apiResponse);
                }


                c1.CustomerId = customid;
                c1.FlowerId = id;
                c1.ItemPrice = f1.UnitPrice;
                c1.Quantity = 1;

            }

            ViewBag.fid = id;

            return View(c1);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(Cart c1)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            var id = HttpContext.Session.GetInt32("Userid"); 

            Flower f1 = new Flower();

            c1.CustomerId = id;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(c1), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("api/Flower/AdditemtoCart", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("CustomerCart", "Cart");
        }
        
    }
}
