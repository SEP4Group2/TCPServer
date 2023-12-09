using IoTBridge.Communicators.PlantApi.DTOs.Requests;
using IoTBridge.IncomingData.Iot;

namespace IoTBridge.Communicators.PlantApi.Helper;

public class PlantApiCommunicatorHelper
{
    public static PlantDataCreationDTO ConvertPlantDataToDataApi(PlantData plantData)
    {
        return new PlantDataCreationDTO()
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