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
      

        public async Task<IActionResult> Index()
        {
            var books = await context.Books.Include(b => b.Category).ToListAsync();
            ViewBag.TotalBooks = await context.Books.CountAsync();
            ViewBag.TotalUsers = await context.Users.CountAsync();
            ViewBag.TotalCategories = await context.Categories.CountAsync();
            return View("Index", books);
        }

        public async Task<IActionResult> AboutUs()
        {
            ViewBag.TotalBooks = await context.Books.CountAsync();
            ViewBag.TotalUsers = await context.Users.CountAsync();
            ViewBag.TotalCategories = await context.Categories.CountAsync();
            return View("AboutUs");
        }

        public IActionResult Privacy()
        {
            return View("Privacy");
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}