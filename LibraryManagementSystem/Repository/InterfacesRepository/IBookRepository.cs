namespace LibraryManagementSystem.Repository.InterfacesRepository
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<Book?> GetWithCategoryAsync(int id);
        Task<IEnumerable<Book>> GetAvailableBooksAsync();
        Task<IEnumerable<Book>> GetAllWithCategoryAsync();
        Task AddAsync(Book book);
        void Update(Book book);
        void Delete(Book book);
    }
}
