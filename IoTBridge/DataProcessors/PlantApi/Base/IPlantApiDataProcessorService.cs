using IoTBridge.DataProcessors.PlantApi.Data;

namespace IoTBridge.DataProcessors.PlantApi.Base
{
    public interface IPlantApiDataProcessorService
    {
        void WaterPlant(WaterPlantData waterPlantData);
    }
}