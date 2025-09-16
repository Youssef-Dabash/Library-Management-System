namespace LibraryManagementSystem.Repository.InterfacesRepository
{
    public interface IBorrowingRepository
    {
        Task<IEnumerable<Borrowing>> GetAllAsync();
        Task<IEnumerable<Borrowing>> GetAllWithUserAndBookAsync();
        Task<Borrowing?> GetByIdWithDetailsAsync(int id);
        Task<Borrowing?> GetByIdAsync(int id);
        Task AddAsync(Borrowing borrowing);
        void Update(Borrowing borrowing);
        void Delete(Borrowing borrowing);
    }
}
