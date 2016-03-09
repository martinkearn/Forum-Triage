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
            //read data files
            var users = System.IO.File.ReadAllLines(@"..\data\Users.txt");
            var tags = System.IO.File.ReadAllLines(@"..\data\Tags.txt");

            //construct view model
            var viewModel = new QuestionsSearchViewModel()
            {
                Users = users,
                Tags = tags
            };

            return View(viewModel);
        }

        //
        // POST: /Home/Questions
        [HttpPost]
        public async Task<IActionResult> Questions(QuestionsSearchViewModel search)
        {
            //prepare query strings for api call
            var tagsForQuery = string.Empty;
            if (search.Tags != null)
            {
                foreach (var tag in search.Tags)
                {
                    tagsForQuery += tag + ";";
                }
                tagsForQuery = tagsForQuery.TrimEnd(';');
            }

            var usersForQuery = string.Empty;
            if (search.Users != null)
            {
                foreach (var owner in search.Users)
                {
                    usersForQuery += owner + ";";
                }
                usersForQuery = usersForQuery.TrimEnd(';');
            }

            //construct api url
            var apiUrl = (string.Format("users/{0}/questions/unanswered?pagesize={1}&order=desc&sort=activity&site=stackoverflow",
                usersForQuery,
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
