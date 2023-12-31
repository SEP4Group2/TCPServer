using System.Net.Sockets;
using IoTBridge.Communicators.Iot;
using IoTBridge.Communicators.Iot.Base;
using IoTBridge.Communicators.Iot.Data;
using IoTBridge.Communicators.PlantApi;
using IoTBridge.Communicators.PlantApi.Base;
using IoTBridge.Communicators.PlantApi.Requests;
using IoTBridge.Connection.Base;
using IoTBridge.Connection.Base.Data;
using IoTBridge.Connection.Entities;
using IoTBridge.DataCaching;
using IoTBridge.DataCaching.Base;
using IoTBridge.DataProcessors.Iot.Base;
using IoTBridge.DataProcessors.Iot.Data;
using IoTBridge.DataProcessors.Iot.Data.Extensions;
using IoTBridge.Shared.OutgoingData;

namespace IoTBridge.DataProcessors.Iot.Service
{
    public class IotDataProcessorService : IIotDataProcessorService
    {
        private const int MAX_CACHED_DATA = 1;
        private readonly ITcpConnectionService tcpConnectionService;
        private readonly IIotCommunicator iotCommunicator;
        private readonly IPlantApiCommunicator plantApiCommunicator;
        private readonly IPlantDataApiCache plantDataApiCache;
    
        public IotDataProcessorService(ITcpConnectionService tcpConnectionService, IIotCommunicator iotCommunicator, IPlantApiCommunicator plantApiCommunicator, IPlantDataApiCache plantDataApiCache)
        {
            this.tcpConnectionService = tcpConnectionService;
            this.iotCommunicator = iotCommunicator;
            this.plantApiCommunicator = plantApiCommunicator;
            this.plantDataApiCache = plantDataApiCache;
        }

        public void ForwardPlantDataToCache(int connectionId, PlantData plantData)
        {
            PlantDataApiDTO convertedPlantData = plantData.ConvertPlantDataToDataApi();
            plantDataApiCache.CachePlantData(connectionId, convertedPlantData);
            if (!plantDataApiCache.HasConnectionReachedMaxCache(connectionId, MAX_CACHED_DATA))
            {
                return;
            }

            List<PlantDataApiDTO> cachedPlantData = plantDataApiCache.GetCachedDataByConnectionId(connectionId);
            Task.Run(async () =>
            {
                await ForwardPlantDataToApiAsync(connectionId, cachedPlantData);
            });
        }

        public void UpdateDeviceStatus(int connectionId, bool status)
        {
            UpdateDeviceStatusAsync(connectionId, status).ConfigureAwait(false);
        }
        
        public async Task UpdateDeviceStatusAsync(int connectionId, bool status)
        {
            UpdateDeviceStatusRequest deviceStatusRequest = new UpdateDeviceStatusRequest()
            {
                DeviceId = connectionId,
                Status = status
            };
            
            EmptyCommunicatorResult updateDeviceStatusResult = await plantApiCommunicator.UpdateDeviceStatus(deviceStatusRequest);
            if (updateDeviceStatusResult.HasError)
            {
                Console.WriteLine(updateDeviceStatusResult.Error);
                return;
            }
            Console.WriteLine($"Successfully updated status for device with id: {connectionId}");
            
            tcpConnectionService.CloseAndRemoveConnection(connectionId);
            Console.WriteLine("Successfully removed connection with id: " + connectionId);
        }

        public void RegisterDevice(TcpClient client, RegistrationData registrationData, int invalidId)
        {
            

            Task.Run(async () =>
            {
                await RegisterDeviceAsync(client, registrationData, invalidId);
            });
        }
    
        private async Task RegisterDeviceAsync(TcpClient client, RegistrationData registrationData, int invalidId)
        {
            Console.WriteLine($"Trying to registrate device with remote endpoint: {client.Client.RemoteEndPoint}");
            var existingConnectionIds = tcpConnectionService.GetExistingIds();
            int deviceId = registrationData.DeviceId;
            Console.WriteLine($"The recieved device id: {deviceId}");
            
            if(deviceId == invalidId || !existingConnectionIds.Contains(deviceId))
            {
                Console.WriteLine("Generating a new id for the device...");
                deviceId = NewIdGenerator.GenerateNewId(existingConnectionIds);
                Console.WriteLine("trying to forward the registration to plantApi...");
                EmptyCommunicatorResult plantApiDeviceRegistrationResult = await plantApiCommunicator.RegisterDevice(deviceId);
                if (plantApiDeviceRegistrationResult.HasError)
                {
                    Console.WriteLine($"Error happend in the plantapi: {plantApiDeviceRegistrationResult.Error}");
                    return;
                }
                Console.WriteLine("The registration happend to plantApi...");
                Console.WriteLine($"Successfully created new id: {deviceId} for device with remote endpoint: {client.Client.RemoteEndPoint}");
            }
            else if(deviceId > 0 && !existingConnectionIds.Contains(deviceId))
            {
                var status = new UpdateDeviceStatusRequest()
                {
                    DeviceId = deviceId,
                    Status = true
                };
                
                EmptyCommunicatorResult plantApiDeviceUpdateResult = await plantApiCommunicator.UpdateDeviceStatus(status);
                if (plantApiDeviceUpdateResult.HasError)
                {
                    Console.WriteLine(plantApiDeviceUpdateResult.Error);
                    return;
                }
            }

            ITcpConnection newConnection = new TcpConnection()
            {
                Client = client,
                ConnectionId = deviceId
            };
            tcpConnectionService.AddConnection(newConnection); 
            Console.WriteLine("Sending the registration id to the client...");
            iotCommunicator.SendRegistrationId(deviceId);
            Console.WriteLine("Sending the registration id to the client...Done");

            var idMessageToDisplay = "";
            if (deviceId < 10)
            {
                idMessageToDisplay = "0" + deviceId;
            }
            else
            {
                idMessageToDisplay = deviceId.ToString();
            }

            await Task.Delay(3000);
            Console.WriteLine("Sending a message to display id");
            iotCommunicator.SendMessage(deviceId, $"ID{idMessageToDisplay}");
            Console.WriteLine("Sending a message to display id...Done");
            
            await Task.Delay(3000);
            Console.WriteLine("Sending a message to start sending data");
            iotCommunicator.SendAction(deviceId, IotActions.DATA);
            Console.WriteLine("Sending a message to start sending data...Done");
        }

        private async Task ForwardPlantDataToApiAsync(int connectionId, List<PlantDataApiDTO> plantData)
        {
            Console.WriteLine($"Trying to forward cached plant data to api for connection: {connectionId}");
            var plantDataRequest = new PlantDataCreationRequest()
            {
                PlantDataApi = plantData
            };
            
            EmptyCommunicatorResult sendPlantDataResult = await plantApiCommunicator.SendPlantData(plantDataRequest);
            if (sendPlantDataResult.HasError)
            {
                Console.WriteLine($"Error when sending data to api: {sendPlantDataResult.Error}");
                return;
            } 
            
            Console.WriteLine("Successfully forwarded cached plant data to api");
            plantDataApiCache.ClearCacheByConnectionId(connectionId);
        }
    }
}