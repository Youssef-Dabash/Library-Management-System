namespace LibraryManagementSystem.Repository.InterfacesRepository
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> FindAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetUserWithDetailsAsync(int id);
        Task<IEnumerable<User>> GetAllWithMembershipAsync();
        Task<User?> GetByUserNameAndPasswordAsync(string userName, string password);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
    }
}
