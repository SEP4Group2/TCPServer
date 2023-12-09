using IoTBridge.DataProcessors.Iot.Base;

namespace IoTBridge.DataProcessors.Iot.Data;

public class TcpReceivedData : ITcpReceivedData
{
    public TcpDataTypes DataType { get; set; }
    public string Data { get; set; }
}