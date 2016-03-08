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
using Microsoft.AspNet.Http.Internal;
using Newtonsoft.Json.Linq;
using ForumTriage_Web.Models;
using Newtonsoft.Json;

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
                Owners = owners,
                Tags = tags
            };

            return View(viewModel);
        }

        //
        // POST: /Home/Search
        [HttpPost]
        public async Task<IActionResult> Search(SearchViewModel vm)
        {
            //prepare query strings for api call
            var tagsForQuery = string.Empty;
            foreach (var tag in vm.Tags)
            {
                tagsForQuery += tag + ";";
            }
            tagsForQuery.TrimEnd(';');

            //TO DO need to get list of user IDs from the SO api here. This way we can add these to the search request using advanced search or store ids in the data file and add a tool in this app to resolve display names to ids
            var ownersForQuery = string.Empty;
            foreach (var owner in vm.Owners)
            {
                ownersForQuery += owner + ";";
            }
            ownersForQuery.TrimEnd(';');

            //call SO api
            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var httpClient = new HttpClient(handler))
            {
                //The advanced search api allow querying on both tags and user, but the user is an ID not display name: http://api.stackexchange.com/docs/advanced-search
                var apiUrl = (string.Format("http://api.stackexchange.com/2.2/search/advanced?order=desc&sort=activity&accepted=False&tagged={0}&site=stackoverflow", tagsForQuery));

                //setup HttpClient
                httpClient.BaseAddress = new Uri(apiUrl);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //make request
                var responseString = await httpClient.GetStringAsync(apiUrl);

                //parse json string to object
                List<StackOverflowQuestion> filteredQuestions = new List<StackOverflowQuestion>();
                JObject response = JObject.Parse(responseString);
                IList<JToken> results = response["items"].Children().ToList();
                foreach (var result in results)
                {
                    StackOverflowQuestion question = JsonConvert.DeserializeObject<StackOverflowQuestion>(result.ToString());
                    //filter by owner
                    if (vm.Owners.Contains(question.owner.display_name))
                    {
                        filteredQuestions.Add(question);
                    }
                }

                //filter to select owners



                //write to view
                ViewData["Result"] = "fibble";
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
