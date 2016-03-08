using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.IO.Compression;
using System.Text;
using ForumTriage_Web.ViewModels.Home;
using System.Net;

namespace ForumTriage_Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            return View();
        }

        //
        // GET: /Home/Search
        [HttpGet]
        public IActionResult Search()
        {
            //read data files
            var owners = System.IO.File.ReadAllLines(@"..\data\Owners.txt");
            var tags = System.IO.File.ReadAllLines(@"..\data\Tags.txt");

            //construct view model
            var viewModel = new SearchViewModel()
            {
                Owners = owners.ToList<string>(),
                Tags = tags.ToList<string>()
            };

            return View(viewModel);
        }

        //
        // POST: /Home/Search
        [HttpPost]
        public async Task<IActionResult> Search(string place)
        {
            var tags = new List<string>();

            //call SO api
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var httpClient = new HttpClient(handler))
            {
                var apiUrl = (string.Format("http://api.stackexchange.com/2.2/search/advanced?order=desc&sort=activity&accepted=False&tagged={0}&site=stackoverflow", tags[0]));

                //setup HttpClient
                httpClient.BaseAddress = new Uri(apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //make request
                var response = await httpClient.GetStringAsync(apiUrl);

                //write to view
                ViewData["Result"] = response;
            }


            //using (var httpClient = new HttpClient())
            //{
            //    var apiUrl = (string.Format("http://api.stackexchange.com/2.2/search/advanced?order=desc&sort=activity&accepted=False&tagged={0}&site=stackoverflow", tags[0]));

            //    //setup HttpClient
            //    httpClient.BaseAddress = new Uri(apiUrl);
            //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    //make request
            //    var response = string.Empty;
            //    using (var responseStream = await httpClient.GetStreamAsync(apiUrl))
            //    {
            //        MemoryStream output = new MemoryStream();
            //        using (GZipStream gzipStream = new GZipStream(responseStream, CompressionMode.Decompress))
            //        {
            //            gzipStream.CopyTo(output);
            //        }
            //        response = Encoding.UTF8.GetString(output.ToArray());
            //    }

            //    //write to view
            //    ViewData["Result"] = response;
            //}
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
