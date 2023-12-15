using IoTBridge.Core.Data;
using IoTBridge.Core.Listener;

namespace IoTBridge.Listeners.Base
{
    public interface IHttpListener<TReceivedData>: IListener
        where TReceivedData : IRecievedData
    {
        Action<TReceivedData> OnMessageReceived { get; set; }
    }
}