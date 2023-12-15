using System.Net.Sockets;
using IoTBridge.Core.Connection.Entities;

namespace IoTBridge.Connection.Base.Data
{
    public interface ITcpConnection : IConnection<TcpClient>
    {
        int ConnectionId { get; set; }
    }
}