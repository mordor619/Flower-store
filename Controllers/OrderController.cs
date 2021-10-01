
using FlowerStore.ProjModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FlowerClient.Controllers
{
    public class OrderController : Controller
    {
        string Baseurl = "https://localhost:44318/";

        [HttpGet]
        public async Task<IActionResult> ViewOrders()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            List<OrderDetail> CartInfo = new List<OrderDetail>();

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(Baseurl);

                    client.DefaultRequestHeaders.Clear();
                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                    HttpResponseMessage Res = await client.GetAsync("api/Flower/AllOrders");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        CartInfo = JsonConvert.DeserializeObject<List<OrderDetail>>(EmpResponse);

                    }
                    //returning the employee list to view  
                    return View(CartInfo);
                }
            }

        [HttpGet]
        public async Task<IActionResult> YourOrders()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            List<OrderDetail> CartInfo = new List<OrderDetail>();
            //var customid = HttpContext.Session.GetInt32("custId");
            var customid = 103; //Get From Session
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Flower/OrderdetailsbyCustomerId?id=" + customid);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    CartInfo = JsonConvert.DeserializeObject<List<OrderDetail>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(CartInfo);
            }
        }

        [HttpGet]
        public async Task<ActionResult> UpdateStatus(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            OrderDetail fl = new OrderDetail();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                using (var response = await httpClient.GetAsync("api/Flower/OrderByOrderID?id=" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    fl = JsonConvert.DeserializeObject<OrderDetail>(apiResponse);
                }
            }
            return View(fl);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStatus(OrderDetail f)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            ViewBag.Usertype = HttpContext.Session.GetString("Usertype");

            OrderDetail o1 = new OrderDetail();

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(Baseurl);
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(f), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("api/Flower/UpdateStatusForOrders", content1))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    
                }
            }
            return RedirectToAction("ViewOrders");
        }

    }
}
