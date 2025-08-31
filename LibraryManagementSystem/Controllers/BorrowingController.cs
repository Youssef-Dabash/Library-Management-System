using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowingController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(nameof(Index));
        }
    }
}
