using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        ApplicationDbContext context;
        public CategoryController(ApplicationDbContext _context)
        {
            context = _context;

        }

        public IActionResult Index()
        {
            var categories = context.Categories.Include(c => c.Books).ToList();
            return View("Index", categories);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View("AddCategory");
        }

        [HttpPost]
        public IActionResult SaveCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult DetailsCategory(int id)
        {
            var category = context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return View("DetailsCategory", category);
        }

        public IActionResult DeleteCategory(int id)
        {
            Category getCategory = context.Categories.Find(id);
            if (getCategory != null)
            {
                context.Categories.Remove(getCategory);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
