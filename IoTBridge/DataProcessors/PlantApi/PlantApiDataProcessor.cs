using IoTBridge.Core.JsonCaster;
using IoTBridge.DataProcessors.Base;
using IoTBridge.DataProcessors.PlantApi.Base;
using IoTBridge.DataProcessors.PlantApi.Data;
using IoTBridge.DataProcessors.PlantApi.Data.DeserializationResults;
using IoTBridge.IncomingData.PlantApi;

namespace IoTBridge.DataProcessors.PlantApi
{
    public class PlantApiDataProcessor : AHttpListenerDataProcessor<IHttpReceivedData>
    {
        private readonly IPlantApiDataProcessorService plantApiDataProcessorService;
        public PlantApiDataProcessor(IPlantApiDataProcessorService plantApiDataProcessorService)
        {
            this.plantApiDataProcessorService = plantApiDataProcessorService;
        }
            
        protected override void ProcessData(IHttpReceivedData data)
        {
            switch (data.DataType)
            {
                case HttpDataTypes.WaterPlant:
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