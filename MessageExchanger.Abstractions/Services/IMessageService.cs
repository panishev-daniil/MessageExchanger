using MessageExchanger.Abstractions.Models;

namespace MessageExchanger.Abstractions.Services
{
    public interface IMessageService 
    {
        public Task SendMessage(Message message);
        public Task<IEnumerable<Message>> GetByDateRange(DateRange dateRange);
    }
}
