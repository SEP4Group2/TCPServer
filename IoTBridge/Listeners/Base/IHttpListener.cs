using IoTBridge.Core.Listener;
using IoTBridge.DataProcessors.PlantApi.Base;

namespace IoTBridge.Listeners.Base;

public interface IHttpListener : IListener
{
    Action<IHttpReceivedData> OnMessageReceived { get; set; }
}