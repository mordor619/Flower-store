using FlowerStore.ProjModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlowerStore.Controllers
{
    public class LoginController : Controller
    {
        string Baseurl = "https://localhost:44318/";

        public IActionResult Login()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Customer cus)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            using (var httpClient = new HttpClient())
            {
                //type = "User";
                httpClient.BaseAddress = new Uri(Baseurl);

                using (var response = await httpClient.GetAsync("api/Flower/CustomerLogin?tempPhone=" + cus.Phone + "&tempPass=" + cus.Password + "&tempType=" + cus.Vendor))
                {
                    int apiResponse = (int)response.StatusCode;

                    //Console.WriteLine("\n this is what you are looking for \n" + apiResponse + " \nend\n");

                    if (apiResponse == 200)
                    {
                        
                        string apiResponseCus = await response.Content.ReadAsStringAsync();
                        Customer cust = JsonConvert.DeserializeObject<Customer>(apiResponseCus);

                        HttpContext.Session.SetString("Username", cust.Name);
                        HttpContext.Session.SetString("Usertype", cust.Vendor);
                        HttpContext.Session.SetInt32("Userid", cust.Id);


                        ViewBag.Username = HttpContext.Session.GetString("Username");
                        ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

                        return RedirectToAction("Home", "Sitehome");
                    }

                    
                }
            }

            ViewBag.error = "**Credentials are wrong";

            return View();
        }

        public IActionResult Logout()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");


            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        public IActionResult GoBack()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            return RedirectToAction("Home", "Sitehome");
        }


    }
}
