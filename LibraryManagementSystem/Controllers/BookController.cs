using LibraryManagementSystem.Data;
using LibraryManagementSystem.UnitOfWorks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

public class BookController : Controller
{
    private IWebHostEnvironment webHostEnvironment;
    private UnitOfWork unitOfWork;
    public BookController(UnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment)
    {
        unitOfWork = _unitOfWork;
        webHostEnvironment = _webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var books = await unitOfWork.Books.GetAllWithCategoryAsync();
        return View("Index", books);
    }

    [HttpGet]
    public async Task<IActionResult> AddBook()
    {
        ViewBag.Categories = await unitOfWork.Categories.GetAllAsync();
        ViewBag.Borrowings = await unitOfWork.Borrowings.GetAllAsync();
        return View("AddBook");
    }

    [HttpPost]
    public async Task<IActionResult> SaveBook(Book book, IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            if(book.CategoryId == 0) book.CategoryId = 5;
            if(book.AvailableCopies == null) book.AvailableCopies = 1;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                book.Image = "/Images/" + uniqueFileName;
            }

            await unitOfWork.Books.AddAsync(book);
            await unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        ViewBag.Categories = await unitOfWork.Categories.GetAllAsync();
        ViewBag.Borrowings = await unitOfWork.Borrowings.GetAllAsync();
        return View("AddBook", book);
    }

    public async Task<IActionResult> DetailsBook(int id)
    {
        var book = await unitOfWork.Books.GetWithCategoryAsync(id);

        if (book == null) return NotFound();
        return View("DetailsBook", book);
    }

    [HttpGet]
    public async Task<IActionResult> EditBook(int id)
    {
        var getBook = await unitOfWork.Books.GetByIdAsync(id);
        if (getBook == null) return NotFound();

        ViewBag.Categories = await unitOfWork.Categories.GetAllAsync();
        return View("EditBook", getBook);
    }

    [HttpPost]
    public async Task<IActionResult> EditBook(int id, Book bookFromRequest, IFormFile? ImageFile)
    {
        if (id != bookFromRequest.BookId) return BadRequest();

        if (ModelState.IsValid)
        {
            var getBook = await unitOfWork.Books.GetByIdAsync(id);
            if (getBook == null) return NotFound();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                getBook.Image = "/Images/" + uniqueFileName;
            }

            getBook.Title = bookFromRequest.Title;
            getBook.Author = bookFromRequest.Author;
            getBook.ISBN = bookFromRequest.ISBN;
            getBook.PublishYear = bookFromRequest.PublishYear;
            getBook.AvailableCopies = bookFromRequest.AvailableCopies;
            getBook.Description = bookFromRequest.Description;
            getBook.CategoryId = bookFromRequest.CategoryId == 0 ? 5 : bookFromRequest.CategoryId;

            unitOfWork.Books.Update(getBook);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        ViewBag.Categories = await unitOfWork.Categories.GetAllAsync();
        return View(bookFromRequest);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var getBook = await unitOfWork.Books.GetByIdAsync(id);
        unitOfWork.Books.Delete(getBook);
        await unitOfWork.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> IncreaseCopies(int id)
    {
        var book = await unitOfWork.Books.GetByIdAsync(id);
        if (book == null) return NotFound();

        book.AvailableCopies += 1;
        await unitOfWork.SaveChangesAsync();

        return Json(new { success = true, newCopies = book.AvailableCopies });
    }
}