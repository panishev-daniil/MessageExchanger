using MessageExchanger.Abstractions.Models;
using MessageExchanger.Shared;

namespace MessageExchanger.Abstractions.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        public Task<IEnumerable<Message>> GetByDateRangeAsync(DateRange dateRange);
    }
}
