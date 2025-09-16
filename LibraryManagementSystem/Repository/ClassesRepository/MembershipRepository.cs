using LibraryManagementSystem.Data;
using LibraryManagementSystem.Repository.InterfacesRepository;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repository.ClassesRepository
{
    public class MembershipTierRepository : IMembershipRepository
    {
        private readonly ApplicationDbContext context;

        public MembershipTierRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<MembershipTier?> FindAsync(int id)
        {
            return await context.MembershipTiers.FindAsync(id);
        }

        public async Task<MembershipTier?> FirstOrDefaultAsync(int id)
        {
            return await context.MembershipTiers
                .FirstOrDefaultAsync(m => m.TierId == id);
        }

        public async Task<IEnumerable<MembershipTier>> ToListAsync()
        {
            return await context.MembershipTiers.ToListAsync();
        }

        public void Update(MembershipTier tier)
        {
            context.MembershipTiers.Update(tier);
        }
    }

}
