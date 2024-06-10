using Npgsql;

namespace MessageExchanger.Shared
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly string _connectionString;

        protected Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected NpgsqlConnection GetConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public abstract Task CreateAsync(T entity);
        public abstract Task<T> GetByIdAsync(Guid id);
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task UpdateAsync(T entity);
        public abstract Task DeleteAsync(Guid id);
    }
}
