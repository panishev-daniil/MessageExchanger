using AutoMapper;
using MessageExchanger.Abstractions.Models;
using MessageExchanger.Abstractions.Services;
using MessageExchanger.WEB.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace MessageExchanger.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly WebSocketConnectionService _webSocketConnectionService;
        private readonly IMapper _mapper;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageService messageService, IMapper mapper, WebSocketConnectionService webSocketConnectionService, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _webSocketConnectionService = webSocketConnectionService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var id = _webSocketConnectionService.AddSocket(webSocket);

                _logger.Log(LogLevel.Information, "Connection is open");

                await HandleWebSocket(HttpContext, webSocket, _webSocketConnectionService, id);
            }
            else
            {
                _logger.Log(LogLevel.Error, "Request is not WebSocket");
                HttpContext.Response.StatusCode = 400;
            }
        }

        [HttpPost("/send")]
        public async Task<IActionResult> SendMessage(CreateMessageDto createMessageDto)
        {
            try
            {
                var newMessage = _mapper.Map<Message>(createMessageDto);

                await _messageService.SendMessage(newMessage);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesByDateRange([FromQuery] DateTime dateTimeStart, [FromQuery] DateTime dateTimeEnd)
        {
            try
            {
                var dateRange = new DateRange(dateTimeStart, dateTimeEnd);

                var dates = await _messageService.GetByDateRange(dateRange);

                return Ok(dates);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private async Task HandleWebSocket(HttpContext context, WebSocket webSocket, WebSocketConnectionService connectionManager, string id)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var messageIndex = 0;

            while (!result.CloseStatus.HasValue)
            {
                _logger.LogInformation("Loop iteration started");
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                var createMessageDto = new CreateMessageDto() { Content = message, SentAt = DateTime.UtcNow };
                await SendMessage(createMessageDto);

                var MessageDto = new MessageDto() { MessageIndex = messageIndex, Content = message, SentAt = DateTime.UtcNow };

                message = JsonConvert.SerializeObject(MessageDto);
                messageIndex++;

                await BroadcastMessage(connectionManager, message);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await connectionManager.RemoveSocketAsync(id);
        }

        private async Task BroadcastMessage(WebSocketConnectionService connectionManager, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);

            var sockets = connectionManager.GetAll().Values;

            foreach (var socket in sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    try
                    {
                        await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch (WebSocketException ex)
                    {
                        _logger.LogError(ex.Message);
                        await connectionManager.RemoveSocketAsync(socket);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }
        }
    }
}
