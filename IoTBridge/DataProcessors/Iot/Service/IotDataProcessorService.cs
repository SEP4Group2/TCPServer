using System.Net.Sockets;
using IoTBridge.Communicators.Base;
using IoTBridge.Communicators.Iot;
using IoTBridge.Communicators.PlantApi;
using IoTBridge.Communicators.PlantApi.DTOs.Requests;
using IoTBridge.Communicators.PlantApi.Helper;
using IoTBridge.Connection.Base;
using IoTBridge.Connection.Entities;
using IoTBridge.DataCaching;
using IoTBridge.DataProcessors.Iot.Base;
using IoTBridge.IncomingData.Iot;

namespace IoTBridge.DataProcessors.Iot.Service
{
    public class IotDataProcessorService : IIotDataProcessorService
    {
        private const int MAX_CACHED_DATA = 10;
        private readonly IIotCommunicator iotCommunicator;
        private readonly ITcpConnectionService tcpConnectionService;
        private readonly IPlantApiCommunicator plantApiCommunicator;
        private readonly IPlantDataCache plantDataCache;
    
        public IotDataProcessorService(ITcpConnectionService tcpConnectionService, IIotCommunicator iotCommunicator, IPlantApiCommunicator plantApiCommunicator, IPlantDataCache plantDataCache)
        {
            this.tcpConnectionService = tcpConnectionService;
            this.iotCommunicator = iotCommunicator;
            this.plantApiCommunicator = plantApiCommunicator;
            this.plantDataCache = plantDataCache;
        }

        public void ForwardPlantDataToCache(int connectionId, PlantData plantData)
        {
            plantDataCache.CachePlantData(connectionId, plantData);
            if (!plantDataCache.HasConnectionReachedMaxCache(connectionId, MAX_CACHED_DATA))
            {
                return;
            }

            List<PlantData> cachedPlantData = plantDataCache.GetCachedDataByConnectionId(connectionId);
            ForwardPlantDataToApi(connectionId, cachedPlantData).ConfigureAwait(false);
        }

        public void UpdateDeviceStatus(int connectionId, bool status)
        {
            UpdateDeviceStatusAsync(connectionId, status).ConfigureAwait(false);
        }
        
        public async Task UpdateDeviceStatusAsync(int connectionId, bool status)
        {
            UpdateDeviceStatus deviceStatus = new UpdateDeviceStatus()
            {
                DeviceId = connectionId,
                Status = status
            };
            EmptyCommunicatorResult updateDeviceStatusResult = await plantApiCommunicator.UpdateDeviceStatus(deviceStatus);
            if (updateDeviceStatusResult.HasError)
            {
                Console.WriteLine(updateDeviceStatusResult.Error);
                return;
            }
            tcpConnectionService.RemoveConnection(connectionId);
            Console.WriteLine($"Successfully updated status for device with id: {connectionId}");
        }

        public void RegisterDevice(TcpClient client, RegistrationData registrationData, int invalidId)
        {
            RegisterDeviceAsync(client, registrationData, invalidId).ConfigureAwait(false);
        }
    
        private async Task RegisterDeviceAsync(TcpClient client, RegistrationData registrationData, int invalidId)
        {
            Console.WriteLine($"Trying to registrate device with remote endpoint: {client.Client.RemoteEndPoint}");
            var existingConnectionIds = tcpConnectionService.GetExistingIds();
            int deviceId = registrationData.Id;
            
            if(deviceId == invalidId || !existingConnectionIds.Contains(deviceId))
            {
                Console.WriteLine("Generating a new id for the device...");
                deviceId = NewIdGenerator.GenerateNewId(existingConnectionIds);
                
                EmptyCommunicatorResult plantApiDeviceRegistrationResult = await plantApiCommunicator.RegisterDevice(deviceId);
                if (plantApiDeviceRegistrationResult.HasError)
                {
                    Console.WriteLine(plantApiDeviceRegistrationResult.Error);
                    return;
                }
                Console.WriteLine($"Successfully created new id: {deviceId} for device with remote endpoint: {client.Client.RemoteEndPoint}");
            }
            else
            {
                var status = new UpdateDeviceStatus()
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
        }

        private async Task ForwardPlantDataToApi(int connectionId, List<PlantData> plantData)
        {
            Console.WriteLine($"Trying to forward cached plant data to api for connection: {connectionId}");
            List<PlantDataApi> plantDataApiList = PlantApiCommunicatorHelper.ConvertPlantDataToPlantDataApi(plantData);
            var plantDataRequest = new PlantDataRequest()
            {
                PlantDataApi = plantDataApiList
            };
            
            EmptyCommunicatorResult sendPlantDataResult = await plantApiCommunicator.SendPlantData(plantDataRequest);
            if (sendPlantDataResult.HasError)
            {
                Console.WriteLine($"Error when sending data to api: {sendPlantDataResult.Error}");
                return;
            } 
            
            Console.WriteLine("Successfully forwarded cached plant data to api");
            plantDataCache.ClearCacheByConnectionId(connectionId);
        }
    }
}