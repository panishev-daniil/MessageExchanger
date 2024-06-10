using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace MessageExchanger.Abstractions.Services
{
    public interface IWebSocketConnectionService
    {
        public string AddSocket(WebSocket socket);

        public ConcurrentDictionary<string, WebSocket> GetAll();

        public WebSocket GetSocketById(string id);

        public Task RemoveSocketAsync(string id);

        public Task RemoveSocketAsync(WebSocket socket);
    }
}
