using System.Net.Sockets;
using IoTBridge.Core.Listener;
using IoTBridge.DataProcessors.Iot.Base;

namespace IoTBridge.Listeners.Base
{
    public interface ITcpListener : IListener
    {
        Action<TcpClient, ITcpReceivedData> OnMessageRecieved { get; set; }
        Action<TcpClient> OnClientDisconnected { get; set; }
    }
}