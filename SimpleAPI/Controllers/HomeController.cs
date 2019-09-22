using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleAPI.Models;
using SimpleAPI.Middlewares;

namespace SimpleAPI.Controllers
{
    public class HomeController : Controller
    {
        //private PeopleContext db;
        private PeopleMiddleware peopleMiddleware;
        public HomeController(PeopleMiddleware pm)
        {
            peopleMiddleware = pm;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Send(string name)
        {
            HttpContext.Request.Headers.Add("Name", name);
            var pers = peopleMiddleware.GetByName(name);
            if (pers == null)
            {
                HttpContext.Response.StatusCode = 403;
                HttpContext.Response.WriteAsync("Access Denied");
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Privacy");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Person person)
        {
            peopleMiddleware.CreatePerson(person);
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View(peopleMiddleware.GetPeople());
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(string name)
        {
            if (name != null)
            {
                Task<Person> pers = peopleMiddleware.GetByNameAsync(name);
                if (pers != null)
                    return View(pers);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(string name)
        {
            if (name != null)
            {
                peopleMiddleware.DeletePerson(name);
                return RedirectToAction("Privacy");
            }
            return NotFound();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
