using MessageExchanger.Abstractions.Models;
using MessageExchanger.Abstractions.Repositories;
using MessageExchanger.Shared;
using MessageExchanger.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace MessageExchanger.DAL.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(IOptions<DatabaseOptions> options, ILogger<MessageRepository> logger) 
            : base(options.Value.ConnectionString)
        {
            _logger = logger;
        }

        public override async Task CreateAsync(Message message)
        {
            if (message.Content is null)
            {
                throw new ClientException("Message content cannot be null"); 
            }

            try
            {
                using (var connection = GetConnection())
                {
                    try
                    {
                        using (var cmd = new NpgsqlCommand("INSERT INTO \"message\" (message_id, content, sent_at) VALUES (@MessageId, @Content, @SentAt)", connection))
                        {
                            _logger.LogInformation("Connect to DB is successful");

                            cmd.Parameters.AddWithValue("MessageId", Guid.NewGuid());
                            cmd.Parameters.AddWithValue("Content", message.Content);
                            cmd.Parameters.AddWithValue("SentAt", message.SentAt);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public override async Task DeleteAsync(Guid id)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    try
                    {
                        using (var cmd = new NpgsqlCommand("DELETE FROM \"message\" WHERE message_id = @MessageId", connection))
                        {
                            _logger.LogInformation("Connect to DB is successful");

                            cmd.Parameters.AddWithValue("MessageId", id);
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public override Task<IEnumerable<Message>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Message>> GetByDateRangeAsync(DateRange dateRange)
        {
            try
            {
                var messages = new List<Message>();
                using (var connection = GetConnection())
                {
                    using (var cmd = new NpgsqlCommand("SELECT message_id, content, sent_at FROM \"message\" WHERE sent_at BETWEEN @StartDate AND @EndDate", connection))
                    {
                        _logger.LogInformation("Connect to DB is successful");

                        cmd.Parameters.AddWithValue("StartDate", dateRange.Start);
                        cmd.Parameters.AddWithValue("EndDate", dateRange.End);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                messages.Add(new Message
                                {
                                    MessageId = reader.GetGuid(0),
                                    Content = reader.GetString(1),
                                    SentAt = reader.GetDateTime(2),
                                });
                            }
                        }
                    }
                }

                _logger.LogInformation("Fetch is successful");
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public override Task<Message> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(Message entity)
        {
            throw new NotImplementedException();
        }
    }
}
