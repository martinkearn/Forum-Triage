using ForumTriage_Web.Models;
using ForumTriage_Web.Services;
using ForumTriage_Web.ViewModels.StackOverflowUsers;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForumTriage_Web.Controllers
{
    public class StackOverflowUsersController : Controller
    {
        private ApplicationDbContext _context;

        public StackOverflowUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StackOverflowUsers(StackOverflowUsersSearchViewModel search)
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
            var vm = new StackOverflowUsersViewModel()
            {
                Users = users
            };

            return View(vm);
        }


    }
}
