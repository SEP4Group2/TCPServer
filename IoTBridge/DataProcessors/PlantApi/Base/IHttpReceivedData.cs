using IoTBridge.Core.Data;
using IoTBridge.DataProcessors.PlantApi.Data;

namespace IoTBridge.DataProcessors.PlantApi.Base
{
    public interface IHttpReceivedData : IRecievedData
    {
        HttpDataTypes DataType { get; set; }
        string Data { get; set; }
    }
}