using FlowerStore.ProjModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FlowerStore.Controllers
{
    public class ManageProfController : Controller
    {
        string Baseurl = "https://localhost:44318/";

        public async Task<IActionResult> ManageProf()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            var id = HttpContext.Session.GetInt32("Userid");

            Customer c1 = new Customer();

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);

                using (var response = await httpClient.GetAsync("api/Flower/CustomerbyId?id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    c1 = JsonConvert.DeserializeObject<Customer>(apiResponse);
                }
            }

            return View(c1);
        }

        [HttpPost]
        public async Task<IActionResult> ManageProf(Customer cus)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(cus), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("api/Flower/UpdateCustomer", content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                }
            }

            HttpContext.Session.SetString("Username", cus.Name);

            return RedirectToAction("Home", "Sitehome");            //file and folder
        }

    }
}
