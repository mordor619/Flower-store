using FlowerStore.ProjModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FlowerStore.Controllers
{
    public class OccassionController : Controller
    {
        string Baseurl = "https://localhost:44318/";

        public IActionResult getFlower()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> getFlower(string name)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            name = Request.Form["category"];
            
            List<Flower> FlowerInfo = new List<Flower>();

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

                List<Flower> FlowerInfoFilter = new List<Flower>();

                foreach(Flower obj in FlowerInfo)
                {
                    if(obj.Occassion.Equals(name))
                    {
                        FlowerInfoFilter.Add(obj);
                    }
                }

                //returning the employee list to view  
                return View(FlowerInfoFilter);
            }

        }

    }
}
