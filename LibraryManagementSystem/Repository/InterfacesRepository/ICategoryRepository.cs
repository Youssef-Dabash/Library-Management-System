namespace LibraryManagementSystem.Repository.InterfacesRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetWithBooksAsync(int id);
        Task AddAsync(Category category);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
