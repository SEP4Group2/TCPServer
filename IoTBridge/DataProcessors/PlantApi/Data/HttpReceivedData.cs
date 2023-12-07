using IoTBridge.DataProcessors.PlantApi.Base;

namespace IoTBridge.DataProcessors.PlantApi.Data;

public class HttpReceivedData : IHttpReceivedData
{
    public HttpDataTypes DataType { get; set; }
    public string Data { get; set; }
}