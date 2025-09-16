using LibraryManagementSystem.Data;
using LibraryManagementSystem.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        UnitOfWork unitOfWork;
        public CategoryController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await unitOfWork.Categories.GetAllAsync();
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
                await unitOfWork.Categories.AddAsync(category);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> DetailsCategory(int id)
        {
            var category = await unitOfWork.Categories.GetWithBooksAsync(id);
            if (category == null) return NotFound();

            return View("DetailsCategory", category);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            await unitOfWork.Categories.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
