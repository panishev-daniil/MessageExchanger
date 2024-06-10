using MessageExchanger.Abstractions.Services;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace MessageExchanger
{
    public class WebSocketConnectionService : IWebSocketConnectionService
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public string AddSocket(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            _sockets.TryAdd(id, socket);
            return id;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public WebSocket GetSocketById(string id)
        {
            _sockets.TryGetValue(id, out var socket);
            return socket;
        }

        public async Task RemoveSocketAsync(string id)
        {
            _sockets.TryRemove(id, out var socket);
            if (socket != null && socket.State == WebSocketState.Open)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketConnectionManager", CancellationToken.None);
            }
        }

        public async Task RemoveSocketAsync(WebSocket socket)
        {
            var id = _sockets.FirstOrDefault(p => p.Value == socket).Key;
            if (id != null)
            {
                await RemoveSocketAsync(id);
            }
        }
    }
}
