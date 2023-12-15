using IoTBridge.Core.Data;

namespace IoTBridge.Shared.IncomingData.Base
{
    public interface IHttpReceivedData<TDataTypes> : IRecievedData
        where TDataTypes : Enum
    {
        TDataTypes DataType { get; set; }
        string Data { get; set; }
    }
}