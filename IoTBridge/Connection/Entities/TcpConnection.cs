using System.Net.Sockets;
using IoTBridge.Connection.Base;
using IoTBridge.Connection.Base.Data;

namespace IoTBridge.Connection.Entities
{
    public class TcpConnection : ITcpConnection
    {
        public int ConnectionId { get; set; }
        public TcpClient Client { get; set; }
    }
}