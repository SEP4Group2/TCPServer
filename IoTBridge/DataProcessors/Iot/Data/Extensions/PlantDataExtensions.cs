using IoTBridge.Shared.OutgoingData;

namespace IoTBridge.DataProcessors.Iot.Data.Extensions
{
    public static class PlantDataExtensions
    {
        public static PlantDataApiDTO ConvertPlantDataToDataApi(this PlantData plantData)
        {
            return new PlantDataApiDTO()
            {
                TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                DeviceId = plantData.DeviceId,
                Humidity = plantData.Humidity,
                Temperature = plantData.Temperature,
                UVLight = plantData.UVLight,
                Moisture = plantData.Moisture,
                TankLevel = plantData.TankLevel
            };
        }
    }
}