using IoTBridge.Core.JsonCaster;
using IoTBridge.DataProcessors.Base;
using IoTBridge.DataProcessors.PlantApi.Base;
using IoTBridge.DataProcessors.PlantApi.Data;
using IoTBridge.DataProcessors.PlantApi.Data.DeserializationResults;
using IoTBridge.Shared.IncomingData.PlantApi.Base;

namespace IoTBridge.DataProcessors.PlantApi
{
    public class PlantApiDataProcessor : AHttpListenerDataProcessor<IPlantApiReceivedData>
    {
        private readonly IPlantApiDataProcessorService plantApiDataProcessorService;
        public PlantApiDataProcessor(IPlantApiDataProcessorService plantApiDataProcessorService)
        {
            this.plantApiDataProcessorService = plantApiDataProcessorService;
        }
            
        protected override void ProcessData(IPlantApiReceivedData data)
        {
            switch (data.DataType)
            {
                case PlantApiDataTypes.WaterPlant:
                    PlantApiWaterPlantDataResult waterPlantDataResult = JsonCasterHelper.DeserializeData<PlantApiWaterPlantDataResult, WaterPlantData>(data.Data);
                    if (waterPlantDataResult.HasError)
                    {
                        Console.WriteLine(waterPlantDataResult.Error);
                        return;
                    }
                    plantApiDataProcessorService.WaterPlant(waterPlantDataResult.Data);
                    break;
            }
        }
    }
}