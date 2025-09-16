using LibraryManagementSystem.Data;
using LibraryManagementSystem.Repository.InterfacesRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository.ClassesRepository
{
    public class BookRepository: IBookRepository
    {
        private readonly ApplicationDbContext context;

        public BookRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Book>> GetAllWithCategoryAsync()
        {
            return await context.Books.Include(b => b.Category).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await context.Books.FindAsync(id);
        }

        public async Task<Book?> GetWithCategoryAsync(int id)
        {
            return await context.Books.Include(b => b.Category)
                                       .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
        {
            return await context.Books
                .Where(b => b.AvailableCopies > 0)
                .ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            await context.Books.AddAsync(book);
        }

        public void Update(Book book)
        {
            context.Books.Update(book);
        }

        public void Delete(Book book)
        {
            context.Books.Remove(book);
        }
    }
}
