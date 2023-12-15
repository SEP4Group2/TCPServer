using IoTBridge.Shared.IncomingData.PlantApi.Base;

namespace IoTBridge.Shared.IncomingData.PlantApi
{
    public class PlantApiReceivedData : IPlantApiReceivedData
    {
        public PlantApiDataTypes DataType { get; set; }
        public string Data { get; set; }
    }
}