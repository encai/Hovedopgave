using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TestAdminCore.Data;
using TestAdminCore.Models;
using TestAdminCore.Data.Migrations;
using static TestAdminCore.Data.Migrations.MyDBContext;

namespace TestAdminCore.Controllers
{
    public class HomeController : Controller
    {

        private MyDBContext _context;

        public HomeController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult GetPerson(string name)
        {
            Person person = _context.People.FirstOrDefault(x => x.FirstName == name);
            return View(person);
        }



        [HttpPost]
        public IActionResult AddPerson(Person person)
        {
            // Let db assign id
            person.Id = Guid.Empty;
            _context.Add(person);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "Administrator")]
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
