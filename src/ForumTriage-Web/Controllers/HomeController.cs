using ForumTriage_Web.Models;
using Microsoft.AspNet.Mvc;

namespace ForumTriage_Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Questions");
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
