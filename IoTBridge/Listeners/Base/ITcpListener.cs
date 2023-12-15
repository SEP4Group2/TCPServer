using System.Net.Sockets;
using IoTBridge.Core.Data;
using IoTBridge.Core.Listener;

namespace IoTBridge.Listeners.Base
{
    public interface ITcpListener<TReceivedData> : IListener
        where TReceivedData : IRecievedData
    {
        Action<TcpClient, TReceivedData> OnMessageRecieved { get; set; }
        Action<TcpClient> OnClientDisconnected { get; set; }
    }
}