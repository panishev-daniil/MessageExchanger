using MessageExchanger.Abstractions.Models;
using MessageExchanger.Abstractions.Repositories;
using MessageExchanger.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace MessageExchanger
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Message>> GetByDateRange(DateRange dateRange)
        {
            _logger.LogInformation("Fetching messages from {Start} to: {End}", 
                dateRange.Start, 
                dateRange.End);
            return await _messageRepository.GetByDateRangeAsync(dateRange);
        }

        public async Task SendMessage(Message message)
        {
            _logger.LogInformation("Message received at {SentAt}, message content: {Content}",
                message.SentAt,
                message.Content);
            await _messageRepository.CreateAsync(message);
        }
    }
}
