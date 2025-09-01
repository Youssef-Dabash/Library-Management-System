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

        public async Task<IActionResult> Index()
        {
            var categories = await context.Categories.Include(c => c.Books)
                .ToListAsync();
            return View("Index", categories);
        }

        public IActionResult AddCategory()
        {
            return View("AddCategory");
        }

        [HttpPost]
        public async Task<IActionResult> SaveCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> DetailsCategory(int id)
        {
            var category = await context.Categories
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();

            return View("DetailsCategory", category);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var getCategory = await context.Categories.FindAsync(id);
            if (getCategory != null)
            {
                context.Categories.Remove(getCategory);
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
