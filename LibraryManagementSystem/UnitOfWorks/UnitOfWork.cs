using LibraryManagementSystem.Data;
using LibraryManagementSystem.Repository.ClassesRepository;
using LibraryManagementSystem.Repository.InterfacesRepository;
using Microsoft.VisualBasic;

namespace LibraryManagementSystem.UnitOfWorks
{
    public class UnitOfWork
    {
        ApplicationDbContext context;
        IUserRepository userRepo { get; set; }
        IMembershipRepository memberRepo { get; set; }  
        IBookRepository bookRepo { get; set; }
        ICategoryRepository categoryRepo { get; set; }
        IBorrowingRepository borrowingRepo { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepo == null)
                    userRepo = new UserRepository(context);
                return userRepo;
            }
        }   

        public IMembershipRepository MembershipTiers
        {
            get
            {
                if (memberRepo == null)
                    memberRepo = new MembershipTierRepository(context);
                return memberRepo;
            }
        }   

        public IBookRepository Books
        {
            get
            {
                if (bookRepo == null)
                    bookRepo = new BookRepository(context);
                return bookRepo;
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                if (categoryRepo == null)
                    categoryRepo = new CategoryRepository(context);
                return categoryRepo;
            }
        }

        public IBorrowingRepository Borrowings
        {
            get
            {
                if (borrowingRepo == null)
                    borrowingRepo = new BorrowingRepository(context);
                return borrowingRepo;
            }
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
