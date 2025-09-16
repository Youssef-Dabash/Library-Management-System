using LibraryManagementSystem.Data;
using LibraryManagementSystem.Repository.InterfacesRepository;
using LibraryManagementSystem.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository.ClassesRepository
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await context.Users.FindAsync(id);

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await context.Users.ToListAsync();
        public Task<User?> FindAsync(int id) =>
            context.Users.FindAsync(id).AsTask();

        public async Task AddAsync(User user) =>
            await context.Users.AddAsync(user);

        public void Update(User user) =>
            context.Users.Update(user);

        public void Delete(User user) =>
            context.Users.Remove(user);

        public async Task<User?> GetUserWithDetailsAsync(int id)
        {
            return await context.Users
               .Include(u => u.MembershipTier)
               .Include(u => u.Borrowings)
               .ThenInclude(b => b.Book)
               .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<IEnumerable<User>> GetAllWithMembershipAsync()
        {
            return await context.Users
                .Include(u => u.MembershipTier)
                .ToListAsync();
        }
        public async Task<User?> GetByUserNameAndPasswordAsync(string userName, string password)
        {
            return await context.Users
                .Include(u => u.MembershipTier)
                .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
        }
    }
}
