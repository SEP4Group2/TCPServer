using IoTBridge.Communicators.Iot;
using IoTBridge.Communicators.PlantApi;
using IoTBridge.Communicators.PlantApi.DTOs.Responses.Results;
using IoTBridge.Connection;
using IoTBridge.Connection.Base;
using IoTBridge.DataCaching;
using IoTBridge.DataProcessors.Iot.Base;
using IoTBridge.DataProcessors.Iot.Service;
using IoTBridge.DataProcessors.PlantApi.Base;
using IoTBridge.DataProcessors.PlantApi.Service;
using IoTBridge.Server;

namespace IoTBridge
{
    public class Program
    {
        private static ITcpConnectionService tcpConnectionService;
        private static IPlantApiCommunicator plantApiCommunicator;
        private static IIotCommunicator iotCommunicator;
        private static IIotDataProcessorService iotDataProcessorService;
        private static IPlantApiDataProcessorService plantApiDataProcessorService;
        private static IPlantDataCache plantDataCache;
        
        public static async Task Main()
        {
            Initialize();
            //await CacheExistingDeviceIds();
            
            RunServer();
        }

        private static void Initialize()
        {
             tcpConnectionService = new TcpConnectionService();
             plantDataCache = new PlantDataCache(100);
             
             plantApiCommunicator = new PlantApiCommunicator("http://localhost:5000/api/");
             iotCommunicator = new IotCommunicator(tcpConnectionService);
             
             iotDataProcessorService = new IotDataProcessorService(tcpConnectionService, iotCommunicator, plantApiCommunicator, plantDataCache);
             plantApiDataProcessorService = new PlantApiDataProcessorService(iotCommunicator);
             
             Console.WriteLine("Services have been initialized");
        }

        private static void RunServer()
        {
            Console.WriteLine("Starting IoTBridge server...");
            IotBridgeServer iotBridgeServer = new IotBridgeServer(tcpConnectionService, iotDataProcessorService, plantApiDataProcessorService);
            iotBridgeServer.Initialize();
            iotBridgeServer.Run();
            Console.WriteLine("IoTBridge server has been started");
        }
        
        private static async Task CacheExistingDeviceIds()
        {
            Console.WriteLine("Retrieving existing device ids...");
            ExistingDeviceIdsResult result = await plantApiCommunicator.GetExistingDeviceIds();
            if (result.HasError)
            {
                Console.WriteLine($"Error occured when trying to get existing device ids: {result.Error}");
                return;
            }
            tcpConnectionService.SetExistingIds(result.Data.DeviceIds);
            Console.WriteLine("Existing device ids have been retrieved and cashed");
        }
    }
}