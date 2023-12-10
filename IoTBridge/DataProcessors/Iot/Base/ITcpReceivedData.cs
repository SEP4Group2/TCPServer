using IoTBridge.Core.Data;
using IoTBridge.DataProcessors.Iot.Data;

namespace IoTBridge.DataProcessors.Iot.Base
{
    public interface ITcpReceivedData : IRecievedData
    {
        TcpDataTypes DataType { get; set; }
        string Data { get; set; }
    }
}