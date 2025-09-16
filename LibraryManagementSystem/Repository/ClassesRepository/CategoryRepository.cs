using LibraryManagementSystem.Data;
using LibraryManagementSystem.Repository.InterfacesRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository.ClassesRepository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ApplicationDbContext context;
        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await context.Categories
                                 .Include(c => c.Books)
                                 .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await context.Categories.FindAsync(id);
        }

        public async Task<Category?> GetWithBooksAsync(int id)
        {
            return await context.Categories
                                 .Include(c => c.Books)
                                 .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task AddAsync(Category category)
        {
            await context.Categories.AddAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category != null)
            {
                context.Categories.Remove(category);
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
