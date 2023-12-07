using System.Net;
using System.Net.Sockets;
using System.Text;
using IoTBridge.Connection.Entities;
using IoTBridge.Core.JsonCaster;
using IoTBridge.DataProcessors.Iot.Base;
using IoTBridge.DataProcessors.Iot.Data;
using IoTBridge.Listeners.Base;
using IoTBridge.Listeners.Results;

namespace IoTBridge.Listeners;

public class TcpListener :  ITcpListener
{
    public Action<TcpClient, ITcpReceivedData> OnMessageRecieved { get; set; }
    public Action<TcpClient> OnClientDisconnected { get; set; }

    private readonly int port;
    private readonly IPAddress ipAddress;
    private readonly int maxPacketSize;

    private System.Net.Sockets.TcpListener tcpListener;

    public TcpListener(IPAddress ipAddress, int port, int maxPacketSize)
    {
        this.ipAddress = ipAddress;
        this.port = port;
        this.maxPacketSize = maxPacketSize;
    }
    public void Initialize()
    {
        tcpListener = new System.Net.Sockets.TcpListener(ipAddress, port);
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
            clientThread.Start(client);
        }
    }

    // Note: if there are some issues with the Network stream reading and writing
    // rewrite it to using this method: https://stackoverflow.com/questions/26079279/writing-reading-string-through-networkstream-sockets-for-a-chat
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
                break;
            }

            if (bytesRead == 0)
            {
                Console.WriteLine("TcpListener ended communication with the tcp client");
                break;
            }

            string convertedData = ConvertDataToString(recievedPacket, bytesRead);
            CreateIotRecievedDataResult receivedDataResult = JsonCasterHelper.DeserializeData<CreateIotRecievedDataResult, TcpReceivedData>(convertedData);
            
            if (receivedDataResult.HasError)
            {
                Console.WriteLine(receivedDataResult.Error);
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