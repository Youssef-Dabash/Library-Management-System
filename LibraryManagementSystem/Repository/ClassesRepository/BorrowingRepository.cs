using LibraryManagementSystem.Data;
using LibraryManagementSystem.Repository.InterfacesRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository.ClassesRepository
{
    public class BorrowingRepository: IBorrowingRepository
    {
        private readonly ApplicationDbContext context;
        public BorrowingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Borrowing>> GetAllAsync()
        {
            return await context.Borrowings.ToListAsync();
        }

        public async Task<IEnumerable<Borrowing>> GetAllWithUserAndBookAsync()
        {
            return await context.Borrowings
                .Include(b => b.User).ThenInclude(u => u.MembershipTier)
                .Include(b => b.Book)
                .ToListAsync();
        }

        public async Task<Borrowing?> GetByIdWithDetailsAsync(int id)
        {
            return await context.Borrowings
                .Include(b => b.User).ThenInclude(u => u.MembershipTier)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.BorrowId == id);
        }

        public async Task<Borrowing?> GetByIdAsync(int id)
        {
            return await context.Borrowings.FindAsync(id);
        }

        public async Task AddAsync(Borrowing borrowing)
        {
            await context.Borrowings.AddAsync(borrowing);
        }

        public void Update(Borrowing borrowing)
        {
            context.Borrowings.Update(borrowing);
        }

        public void Delete(Borrowing borrowing)
        {
            context.Borrowings.Remove(borrowing);
        }
    }
}
