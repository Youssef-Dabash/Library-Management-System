using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        //public ApplicationDbContext()
        //{
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<MembershipTier> MembershipTiers { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User ↔ MembershipTier
            modelBuilder.Entity<User>()
                .HasOne(u => u.MembershipTier)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Borrowing ↔ User
            modelBuilder.Entity<Borrowing>()
                .HasOne(b => b.User)
                .WithMany(u => u.Borrowings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Borrowing ↔ Book
            modelBuilder.Entity<Borrowing>()
                .HasOne(b => b.Book)
                .WithMany(bk => bk.Borrowings)
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            // Book ↔ Category
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Enum Mapping: MembershipTier TierName
            modelBuilder.Entity<MembershipTier>()
                .Property(t => t.TierName)
                .HasConversion<string>(); // يخزن Enum كـ string في الجدول



            modelBuilder.Entity<MembershipTier>().HasData(
               new MembershipTier
               {
                   TierId = 1,
                   TierName = MembershipType.Normal,
                   ExtraBooks = 0,
                   ExtraDays = 0,
                   ExtraPenalty = 0.00m
               },
               new MembershipTier
               {
                   TierId = 2,
                   TierName = MembershipType.Premium,
                   ExtraBooks = 2,
                   ExtraDays = 7,
                   ExtraPenalty = 10.00m
               },
               new MembershipTier
               {
                   TierId  = 3,
                   TierName = MembershipType.VIP,
                   ExtraBooks = 5,
                   ExtraDays = 10,
                   ExtraPenalty = 5.00m
               }
             );
        }
    }
}
