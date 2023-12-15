using System.Net;
using System.Net.Sockets;
using System.Text;
using IoTBridge.Core.JsonCaster;
using IoTBridge.Listeners.Base;
using IoTBridge.Listeners.Iot.Results;
using IoTBridge.Shared.IncomingData.Iot;
using IoTBridge.Shared.IncomingData.Iot.Base;

namespace IoTBridge.Listeners.Iot
{
    public class IotListener :  ITcpListener<IIotReceivedData>
    {
        public Action<TcpClient, IIotReceivedData> OnMessageRecieved { get; set; }
        public Action<TcpClient> OnClientDisconnected { get; set; }

        private readonly int port;
        private readonly IPAddress ipAddress;
        private readonly int maxPacketSize;

        private TcpListener tcpListener;

        public IotListener(IPAddress ipAddress, int port, int maxPacketSize)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.maxPacketSize = maxPacketSize;
        }
        public void Initialize()
        {
            tcpListener = new TcpListener(ipAddress, port);
        }

        public void Run()
        {
            tcpListener.Start();
            Console.WriteLine("TcpListener is up and running");
            while (true)
            {
                Console.WriteLine("TcpListener is waiting for a client");
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("New client connected to an TcpListner");
            
                Thread clientThread = new Thread(() => HandleClientsMessages(client));
                clientThread.Start();
            }
        }

        private void HandleClientsMessages(TcpClient client)
        {
            NetworkStream networkStream = client.GetStream();
        
            byte[] recievedPacket = new byte[maxPacketSize];

            while (true)
            {
                int bytesRead = 0;
                try
                {
                    bytesRead = networkStream.Read(recievedPacket, 0, maxPacketSize);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occured while reading from the stream: {e}");
                    continue;
                }

                if (bytesRead == 0)
                {
                    Console.WriteLine("TcpListener ended communication with the tcp client");
                    break;
                }

                string convertedData = ConvertDataToString(recievedPacket, bytesRead);
            
                CreateIotReceivedDataResult receivedDataResult = JsonCasterHelper.DeserializeData<CreateIotReceivedDataResult, IotReceivedData>(convertedData);
                if (receivedDataResult.HasError)
                {
                    Console.WriteLine($"Error occured when deserializing the iot data: {receivedDataResult.Error}");
                    continue;
                }
            
                OnMessageRecieved.Invoke(client, receivedDataResult.Data);
            }
        
            OnClientDisconnected.Invoke(client);
        }

        private string ConvertDataToString(byte[] data, int count)
        {
            return Encoding.ASCII.GetString(data, 0, count);
        }
    }
}