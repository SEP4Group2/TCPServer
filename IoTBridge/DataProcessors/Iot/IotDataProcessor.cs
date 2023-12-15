using System.Net.Sockets;
using IoTBridge.Core.JsonCaster;
using IoTBridge.DataProcessors.Base;
using IoTBridge.DataProcessors.Iot.Base;
using IoTBridge.DataProcessors.Iot.Data;
using IoTBridge.DataProcessors.Iot.Data.DeserializationResults;
using IoTBridge.Shared.IncomingData.Iot.Base;

namespace IoTBridge.DataProcessors.Iot
{
    public class IotDataProcessor : ATcpListenerDataProcessor<IIotReceivedData>
    {
        private readonly IIotDataProcessorService iotDataProcesorService;
        
        public IotDataProcessor(IIotDataProcessorService iotDataProcesorService)
        {
            this.iotDataProcesorService = iotDataProcesorService;
        }

        public override void HandleData(TcpClient tcpClient, IIotReceivedData data)
        {
            Console.WriteLine($"Received data from TcpClient: {tcpClient.Client.RemoteEndPoint}");
            if (DoesDataHandlingRequireTcpClient(data.DataType))
            {
                HandleTcpClientData(tcpClient, data);
                return;
            }
        
            base.HandleData(tcpClient, data);
        }

        private void HandleTcpClientData(TcpClient tcpClient, IIotReceivedData data)
        {
            if (data.DataType == IotDataTypes.Registration)
            {
                IotRegistrationDataResult registrationDataResult =
                    JsonCasterHelper.DeserializeData<IotRegistrationDataResult, RegistrationData>(data.Data);
                if (registrationDataResult.HasError)
                {
                    Console.WriteLine(registrationDataResult.Error);
                    return;
                }
                iotDataProcesorService.RegisterDevice(tcpClient, registrationDataResult.Data, -1);
            }
        }

        private bool DoesDataHandlingRequireTcpClient(IotDataTypes dataDataType)
        {
            bool doesDataHandlingRequireTcpClient = dataDataType switch
            {
                IotDataTypes.Registration => true,
                _ => false
            };
            return doesDataHandlingRequireTcpClient;
        }

        protected override void ProcessData(IIotReceivedData data)
        {
            switch (data.DataType)
            {
                case IotDataTypes.PlantData:
                    IotPlantDataResult plantDataResult = JsonCasterHelper.DeserializeData<IotPlantDataResult, PlantData>(data.Data);
                    if (plantDataResult.HasError)
                    {
                        Console.WriteLine(plantDataResult.Error);
                        return;
                    }
                    iotDataProcesorService.ForwardPlantDataToCache(plantDataResult.Data.DeviceId, plantDataResult.Data);
                    break;
            }
        }

    }
}