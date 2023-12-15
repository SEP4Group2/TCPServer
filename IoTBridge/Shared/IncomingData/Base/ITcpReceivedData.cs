using IoTBridge.Core.Data;

namespace IoTBridge.Shared.IncomingData.Base
{
    public interface ITcpReceivedData<TDataTypes> : IRecievedData
        where TDataTypes : Enum
    {
        TDataTypes DataType { get; set; }
        string Data { get; set; }
    }
}