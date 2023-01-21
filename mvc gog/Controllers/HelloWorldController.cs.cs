using Microsoft.AspNetCore.Mvc;

namespace mvc_gog.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Welcome(string name, int age, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["Age"] = "You are " + age + " years old.";
            ViewData["NumTimes"] = numTimes;
            return View();
        }
    }
}
