using IoTBridge.IncomingData.PlantApi;

namespace IoTBridge.DataProcessors.PlantApi.Base
{
    public interface IPlantApiDataProcessorService
    {
        void WaterPlant(WaterPlantData waterPlantData);
    }
}