using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowingController : Controller
    {
        ApplicationDbContext context;

        public BorrowingController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var borrowings = context.Borrowings
                .Include(b => b.User)
                .Include(b => b.Book);
            var result = await borrowings.ToListAsync();
            return View("Index", result);
        }

        public IActionResult CheckUser() => View("CheckUser");

        [HttpPost]
        public async Task<IActionResult> CheckUser(User userInput)
        {
            var user = await context.Users
                .Include(u => u.MembershipTier)
                .FirstOrDefaultAsync(u => u.UserName == userInput.UserName && u.Password == userInput.Password);

            if (user == null)
            {
                ViewBag.ShowRegisterButton = true; 
                ModelState.AddModelError("", "❌ Invalid username or password, You can add a new user!!");
                return View("CheckUser");
            }

            if (user.NumBooksAvailable <= 0)
            {
                ModelState.AddModelError("", "⚠️ This user has reached the maximum borrowing limit and cannot borrow more books.");
                return View("CheckUser");
            }


            var borrowing = new Borrowing
            {
                UserId = user.UserId,
                User = user,
                BorrowDate = DateTime.Now
            };

            ViewBag.Books = new SelectList(context.Books.Where(b => b.AvailableCopies > 0), "BookId", "Title");

            return View("AddBorrow", borrowing);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBorrow(Borrowing borrowing)
        {
            var user = await context.Users
                .Include(u => u.MembershipTier)
                .FirstOrDefaultAsync(u => u.UserId == borrowing.UserId);

            var book = await context.Books.FindAsync(borrowing.BookId);
          
            borrowing.BorrowDate = DateTime.Now;
            borrowing.DueDate = borrowing.BorrowDate.AddDays(user.MembershipTier.ExtraDays);
            borrowing.Status = "Pending";
            borrowing.FineAmount = 0;

            user.NumBooksAvailable -= 1;
            book.AvailableCopies -= 1;

            context.Add(borrowing);
            await context.SaveChangesAsync();

            await context.Entry(borrowing).Reference(b => b.User).LoadAsync();
            await context.Entry(borrowing).Reference(b => b.Book).LoadAsync();

            return View("DetailsBorrow", borrowing);
        }

        public async Task<IActionResult> ReturnBorrow(int id)
        {
            var borrowing = await context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .ThenInclude(u => u.MembershipTier)
                .FirstOrDefaultAsync(b => b.BorrowId == id);

            if (borrowing == null)
            {
                return NotFound();
            }

            if (borrowing.Status == "Returned")
            {
                TempData["Message"] = "This borrow has already been returned.";
                return RedirectToAction(nameof(Index));
            }

            borrowing.Status = "Returned";
            borrowing.ReturnDate = DateTime.Now;
            borrowing.User.NumBooksAvailable += 1;
            borrowing.Book.AvailableCopies += 1;

            if (borrowing.DueDate < borrowing.ReturnDate)
            {
                var overdueDays = (borrowing.ReturnDate.Value - borrowing.DueDate).Days;
                borrowing.FineAmount = overdueDays * borrowing.User.MembershipTier.ExtraPenalty;
            }

            context.Update(borrowing);
            await context.SaveChangesAsync();

            TempData["Message"] = "Book returned successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsBorrow(int id)
        {
            var borrowing = await context.Borrowings
                .Include(b => b.User)
                .ThenInclude(u => u.MembershipTier)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.BorrowId == id);

            if (borrowing == null)
            {
                return NotFound();
            }

            return View("DetailsBorrow", borrowing);
        }

    }
}
