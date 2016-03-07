using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ForumTriage_Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //read data files
            var owners = System.IO.File.ReadAllLines(@"..\data\Owners.txt");
            var tags = System.IO.File.ReadAllLines(@"..\data\Tags.txt");

            //call SO api
            using (var httpClient = new HttpClient())
            {
                var apiUrl = (string.Format("http://api.stackexchange.com/2.2/search/advanced?order=desc&sort=activity&accepted=False&tagged={0}&site=stackoverflow", tags[0]));
                //setup HttpClient
                httpClient.BaseAddress = new Uri(apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //make request
                var response = await httpClient.GetAsync(apiUrl);

                //read response and write to view
                var responseContent = await response.Content.ReadAsStreamAsync();
                ViewData["Result"] = responseContent;
            }



            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
