using IoTBridge.Communicators.Iot;
using IoTBridge.Communicators.Iot.Base;
using IoTBridge.Communicators.PlantApi;
using IoTBridge.Communicators.PlantApi.Base;
using IoTBridge.Communicators.PlantApi.Results;
using IoTBridge.Connection;
using IoTBridge.Connection.Base;
using IoTBridge.Core.Server;
using IoTBridge.DataCaching;
using IoTBridge.DataCaching.Base;
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
        private static IPlantDataApiCache plantDataApiCache;
        
        private static IServer iotBridgeServer;
        
        public static async Task Main()
        {
            Initialize();
            bool hasCachedIds = await CacheExistingDeviceIds();
            if (!hasCachedIds)
            {
                return;
            }
            
            RunServer();
        }

        private static void Initialize()
        {
             tcpConnectionService = new TcpConnectionService();
             plantDataApiCache = new PlantDataApiCache(100);
             
             plantApiCommunicator = new PlantApiCommunicator("http://plantapi:5000/");
             iotCommunicator = new IotCommunicator(tcpConnectionService);
             
             iotDataProcessorService = new IotDataProcessorService(tcpConnectionService, iotCommunicator, plantApiCommunicator, plantDataApiCache);
             plantApiDataProcessorService = new PlantApiDataProcessorService(iotCommunicator);
             
             Console.WriteLine("Services have been initialized");
        }

        private static void RunServer()
        {
            Console.WriteLine("Starting IoTBridge server...");
            iotBridgeServer = new IotBridgeServer(tcpConnectionService, iotDataProcessorService, plantApiDataProcessorService);
            iotBridgeServer.Initialize();
            iotBridgeServer.Run();
            Console.WriteLine("IoTBridge server has been started");
        }
        
        private static async Task<bool> CacheExistingDeviceIds()
        {
            await Task.Delay(3000);
            Console.WriteLine("Retrieving existing device ids...");
            
            ExistingDeviceIdsResult result = await plantApiCommunicator.GetExistingDeviceIds();
            if (result.HasError)
            {
                Console.WriteLine($"Error occured when trying to get existing device ids: {result.Error}");
                Console.WriteLine($"Trying to reconnect....");
                return await CacheExistingDeviceIds();
            }
            tcpConnectionService.SetExistingIds(result.Data.DeviceIds);
            Console.WriteLine("Existing device ids have been retrieved and cashed");
            return true;
        }
    }
}