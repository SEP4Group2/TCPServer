using IoTBridge.Communicators.Iot.Base;
using IoTBridge.Communicators.Iot.Data;
using IoTBridge.DataProcessors.PlantApi.Base;
using IoTBridge.DataProcessors.PlantApi.Data;

namespace IoTBridge.DataProcessors.PlantApi.Service
{
    public class PlantApiDataProcessorService : IPlantApiDataProcessorService
    {
        private readonly IIotCommunicator iotCommunicator;
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