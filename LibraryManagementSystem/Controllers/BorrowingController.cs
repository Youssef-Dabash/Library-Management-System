using LibraryManagementSystem.Data;
using LibraryManagementSystem.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowingController : Controller
    {
        UnitOfWork unitOfWork;

        public BorrowingController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var borrowings = await unitOfWork.Borrowings.GetAllWithUserAndBookAsync();

            foreach (var b in borrowings)
            {
                if (b.Status != "Returned" && DateTime.Now > b.DueDate)
                {
                    b.Status = "Overdue";

                    if (b.User?.MembershipTier != null)
                    {
                        int overdueDays = (DateTime.Now - b.DueDate).Days + 1;
                        b.FineAmount = overdueDays * b.User.MembershipTier.ExtraPenalty;
                    }
                }
            }

            await unitOfWork.SaveChangesAsync();

            return View(borrowings);
        }


        public IActionResult CheckUser() => View("CheckUser");

        [HttpPost]
        public async Task<IActionResult> CheckUser(User userInput)
        {
            var user = await unitOfWork.Users
                .GetByUserNameAndPasswordAsync(userInput.UserName, userInput.Password);

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

            var availableBooks = await unitOfWork.Books.GetAvailableBooksAsync();
            ViewBag.Books = new SelectList(availableBooks, "BookId", "Title");

            return View("AddBorrow", borrowing);
        }

        [HttpPost]
        public async Task<IActionResult> SaveBorrow(Borrowing borrowing)
        {
            var user = await unitOfWork.Users.GetUserWithDetailsAsync(borrowing.UserId);
            var book = await unitOfWork.Books.GetByIdAsync(borrowing.BookId);
            

            borrowing.BorrowDate = DateTime.Now;
            borrowing.DueDate = borrowing.BorrowDate.AddDays(user.MembershipTier.ExtraDays);
            borrowing.Status = "Pending";
            borrowing.FineAmount = 0;

            user.NumBooksAvailable -= 1;
            book.AvailableCopies -= 1;

            await unitOfWork.Borrowings.AddAsync(borrowing);
            await unitOfWork.SaveChangesAsync();

            return View("DetailsBorrow", borrowing);
        }

        public async Task<IActionResult> ReturnBorrow(int id)
        {
            var borrowing = await unitOfWork.Borrowings.GetByIdWithDetailsAsync(id);

            if (borrowing == null)  return NotFound();

            if (borrowing.Status == "Returned")
            {
                TempData["Message"] = "This borrow has already been returned.";
                return RedirectToAction(nameof(Index));
            }

            borrowing.Status = "Returned";
            borrowing.ReturnDate = DateTime.Now;
            borrowing.User.NumBooksAvailable += 1;
            borrowing.Book.AvailableCopies += 1;
            borrowing.FineAmount = 0;

            unitOfWork.Borrowings.Update(borrowing);
            await unitOfWork.SaveChangesAsync();

            TempData["Message"] = "Book returned successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsBorrow(int id)
        {
            var borrowing = await unitOfWork.Borrowings.GetByIdWithDetailsAsync(id);

            if (borrowing == null) return NotFound();

            return View("DetailsBorrow", borrowing);
        }

        public async Task<IActionResult> DeleteBorrowing(int id)
        {
            var borrowing = await unitOfWork.Borrowings.GetByIdWithDetailsAsync(id);

            if (borrowing == null) return NotFound();

            unitOfWork.Borrowings.Delete(borrowing);
            await unitOfWork.SaveChangesAsync();

            TempData["Message"] = "Borrow record deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
