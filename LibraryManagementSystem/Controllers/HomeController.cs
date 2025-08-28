using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext _context, ILogger<HomeController> logger)
        {
            context = _context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Book> books = context.Books.Include(b => b.Category).ToList();
            return View("Index", books);
        }

        public IActionResult AboutUs()
        {
            ViewBag.TotalBooks = context.Books.Count();
            ViewBag.TotalUsers = context.Users.Count();
            ViewBag.TotalCategories = context.Categories.Count();
            return View("AboutUs");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}