using LibraryManagementSystem.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

public class BookController : Controller
{
    private IWebHostEnvironment webHostEnvironment;
    private ApplicationDbContext context;
    public BookController(ApplicationDbContext _context, IWebHostEnvironment _webHostEnvironment)
    {
        context = _context;
        webHostEnvironment = _webHostEnvironment;
    }

    public IActionResult Index()
    {
        var books = context.Books?.Include(b => b.Category).ToList();
        return View("Index", books);
    }

    [HttpGet]
    public IActionResult AddBook()
    {
        ViewBag.Categories = context.Categories.ToList();
        ViewBag.Borrowings = context.Borrowings.ToList();
        return View("AddBook");
    }

    [HttpPost]
    public IActionResult SaveBook(Book book, IFormFile? ImageFile)
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

            context.Books.Add(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.Categories = context.Categories.ToList();
        ViewBag.Borrowings = context.Borrowings.ToList();
        return View("AddBook", book);
    }

    public IActionResult DetailsBook(int id)
    {
        var book = context.Books
            .Include(b => b.Category)
            .FirstOrDefault(b => b.BookId == id);

        if (book == null) return NotFound();
        return View("DetailsBook", book);
    }

    [HttpGet]
    public IActionResult EditBook(int id)
    {
        Book getBook = context.Books.Find(id);
        if (getBook == null) return NotFound();

        ViewBag.Categories = context.Categories.ToList();
        return View("EditBook", getBook);
    }

    [HttpPost]
    public IActionResult EditBook(int id, Book bookFromRequest, IFormFile? ImageFile)
    {
        if (id != bookFromRequest.BookId) return BadRequest();

        if (ModelState.IsValid)
        {
            var getBook = context.Books.Find(id);
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

            context.Books.Update(getBook);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        ViewBag.Categories = context.Categories.ToList();
        return View(bookFromRequest);
    }

    [HttpPost]
    public IActionResult DeleteBook(int id)
    {
        var getBook = context.Books.Find(id);
        context.Books.Remove(getBook);
        context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult IncreaseCopies(int id)
    {
        var book = context.Books.Find(id);
        if (book == null) return NotFound();

        book.AvailableCopies += 1;
        context.SaveChanges();

        return RedirectToAction("Index", new { id = id });
    }

}


