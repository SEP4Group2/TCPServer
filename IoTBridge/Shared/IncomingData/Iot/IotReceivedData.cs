using IoTBridge.Shared.IncomingData.Iot.Base;

namespace IoTBridge.Shared.IncomingData.Iot
{
    public class IotReceivedData : IIotReceivedData
    {
        public IotDataTypes DataType { get; set; }
        public string Data { get; set; }
    }
}