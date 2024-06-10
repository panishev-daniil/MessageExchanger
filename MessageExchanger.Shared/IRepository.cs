namespace MessageExchanger.Shared
{
    public interface IRepository<T>
    where T : class
    {
        Task CreateAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
