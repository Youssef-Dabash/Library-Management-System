using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowingController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
