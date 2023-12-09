using IoTBridge.Communicators.Iot;
using IoTBridge.Communicators.Iot.Data;
using IoTBridge.Communicators.PlantApi;
using IoTBridge.DataProcessors.PlantApi.Base;
using IoTBridge.IncomingData.PlantApi;

namespace IoTBridge.DataProcessors.PlantApi.Service
{
    public class PlantApiDataProcessorService : IPlantApiDataProcessorService
    {
        private readonly IIotCommunicator iotCommunicator;
        private readonly IPlantApiCommunicator plantApiCommunicator;
        public PlantApiDataProcessorService(IIotCommunicator iotCommunicator)
        {
            this.iotCommunicator = iotCommunicator;
        }
        public void WaterPlant(WaterPlantData waterPlantData)
        {
            try
            {
                iotCommunicator.SendAction(waterPlantData.DeviceId, IotActions.PUMP);   
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to water plant because: " + e.Message);
            }
        }
    }
}