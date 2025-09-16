using LibraryManagementSystem.Models;
namespace LibraryManagementSystem.Repository.InterfacesRepository
{
    public interface IMembershipRepository
    {
        Task<MembershipTier?> FindAsync(int id);
        Task<MembershipTier?> FirstOrDefaultAsync(int id);
        Task<IEnumerable<MembershipTier>> ToListAsync();
        void Update(MembershipTier tier);
    }
}
