using System.Net;
using System.Net.Sockets;
using IoTBridge.Connection.Base;
using IoTBridge.Core.Server;
using IoTBridge.DataProcessors.Base;
using IoTBridge.DataProcessors.Iot;
using IoTBridge.DataProcessors.Iot.Base;
using IoTBridge.DataProcessors.PlantApi;
using IoTBridge.DataProcessors.PlantApi.Base;
using IoTBridge.Listeners.Base;
using HttpListener = IoTBridge.Listeners.HttpListener;
using TcpListener = IoTBridge.Listeners.TcpListener;

namespace IoTBridge.Server
{
    public class IotBridgeServer : IServer
    {
        private readonly ITcpConnectionService tcpConnectionService;
        private readonly ITcpListener iotListener;
        private readonly IHttpListener plantApiListener;
        private readonly AHttpListenerDataProcessor<IHttpReceivedData> plantApiDataProcessor;
        private readonly ATcpListenerDataProcessor<ITcpReceivedData> iotDataProcessor;
        private readonly IIotDataProcessorService iotDataProcessorService;
    
        private Thread iotListenerThread;
        private Thread plantApiListenerThread;
    
        public IotBridgeServer(ITcpConnectionService tcpConnectionService, IIotDataProcessorService iotDataProcessorService, IPlantApiDataProcessorService plantApiDataProcessorService)
        {
            this.tcpConnectionService = tcpConnectionService;
            this.iotDataProcessorService = iotDataProcessorService;
        
            iotListener = new TcpListener(IPAddress.Any, 3014, 4096);
            plantApiListener = new HttpListener("http://+:5024/", "api/plants");
            plantApiDataProcessor = new PlantApiDataProcessor(plantApiDataProcessorService);
            iotDataProcessor = new IotDataProcessor(iotDataProcessorService);
        }
        public void Initialize()
        {
            iotListener.Initialize();
            plantApiListener.Initialize();

            iotListener.OnMessageRecieved += HandleIotData;
            iotListener.OnClientDisconnected += HandleCloseTcpConnection;
            plantApiListener.OnMessageReceived += HandlePlantApiData;
        
            iotListenerThread = new Thread(iotListener.Run);
            plantApiListenerThread = new Thread(plantApiListener.Run);
        }
    
        public void Run()
        {
            iotListenerThread.Start();
            plantApiListenerThread.Start();
        }

        private void HandleCloseTcpConnection(TcpClient tcpClient)
        {
            int connectionId = tcpConnectionService.GetConnectionIdByTcpClient(tcpClient);
            if (connectionId == -1)
            {
                Console.WriteLine("Client Disconnected, but its connection was not registered properly.");
                return;
            }
            iotDataProcessorService.UpdateDeviceStatus(connectionId, false);
        }

        private void HandleIotData(TcpClient tcpClient, ITcpReceivedData data)
        {
            iotDataProcessor.HandleData(tcpClient, data);
        }
    
        private void HandlePlantApiData(IHttpReceivedData data)
        {
            plantApiDataProcessor.HandleData(data);
        }
    }
}