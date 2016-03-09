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
using ForumTriage_Web.Constants;
using ForumTriage_Web.Services;

namespace ForumTriage_Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            return View();
        }

        //
        // GET: /Home/QuestionsSearch
        public IActionResult QuestionsSearch()
        {
            //read data files into an array of users
            List<User> users = new List<User>();

            using (StreamReader reader = System.IO.File.OpenText(@"..\data\Users.json"))
            {
                JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));

                JArray usersArray = (JArray)o["users"];

                foreach (var u in usersArray)
                {
                    var user = new User()
                    {
                        Name = (string)u["Name"],
                        StackOverflowUserId = (string)u["StackOverflowUserId"],
                        Organisation = (string)u["Organisation"]
                    };

                    users.Add(user);
                }
            }

            //construct view model
            var viewModel = new QuestionsSearchGetViewModel()
            {
                Users = users
            };

            return View(viewModel);
        }

        //
        // POST: /Home/Questions
        [HttpPost]
        public async Task<IActionResult> Questions(QuestionsSearchPostViewModel search)
        {
            //prepare query strings for api call
            var userIdsForQuery = string.Empty;
            if (search.UserIds != null)
            {
                foreach (var userId in search.UserIds)
                {
                    userIdsForQuery += userId + ";";
                }
                userIdsForQuery = userIdsForQuery.TrimEnd(';');
            }

            //construct api url
            var apiUrl = (string.Format("users/{0}/questions/unanswered?pagesize={1}&order=desc&sort=activity&site=stackoverflow",
                userIdsForQuery,
                Constants.Constants.StackOverflowApiPageSize));

            //get results
            var results = await StackOverflowAPIService.CallApi(apiUrl);

            //parse json string to object
            List<StackOverflowQuestion> questions = new List<StackOverflowQuestion>();
            foreach (var result in results)
            {
                StackOverflowQuestion question = JsonConvert.DeserializeObject<StackOverflowQuestion>(result.ToString());
                questions.Add(question);
            }

            //populate view model
            var vm = new QuestionsViewModel()
            {
                Questions = questions
            };

            return View(vm);
        }

        public IActionResult UsersSearch()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Users(UsersSearchViewModel search)
        {
            //construct api url
            var apiUrl = (string.Format("users?pagesize={0}&order=desc&sort=reputation&inname={1}&site=stackoverflow", 
                Constants.Constants.StackOverflowApiPageSize, 
                search.InName));

            //get results
            var results = await StackOverflowAPIService.CallApi(apiUrl);

            //parse json string to object
            List<StackOverflowUser> users = new List<StackOverflowUser>();
            foreach (var result in results)
            {
                StackOverflowUser user = JsonConvert.DeserializeObject<StackOverflowUser>(result.ToString());
                users.Add(user);
            }

            //populate view model
            var vm = new UsersViewModel()
            {
                Users = users
            };

            return View(vm);
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
