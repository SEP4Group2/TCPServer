 using System.Net;
using System.Net.Sockets;
using System.Text;
using Domain.Model;
using TCPServer.util;

namespace TCPServer;

public class TCPServer
{
    private TcpListener tcpListener;
    private Thread listenThread;
    private PlantHttpClient plantHttpClient;
    
    public TCPServer()
    {
        tcpListener = new TcpListener(IPAddress.Any, 23);
        listenThread = new Thread(new ThreadStart(ListenForClients));
        listenThread.Start();
        plantHttpClient = new PlantHttpClient();
    }
    
    
    private void ListenForClients()
    {
        this.tcpListener.Start();

        while (true)
        {
            TcpClient client = this.tcpListener.AcceptTcpClient();
            Thread clientThread = new Thread(HandleClientComm);
            clientThread.Start(client);
        }
    }
    
    
    // public async Task SendCodeToArduinoClient(int portNumber, int code)
    // {
    //     // You need to implement this method to send the code to a specific Arduino client
    //     // Find the client with the specified MAC address and send the code
    //     // Example: Sending the code to the client with the specified MAC address
    //     var targetClient = FirstOrDefault(client => GetMacAddress(client) == macAddress);
    //     
    //     if (targetClient != null)
    //     {
    //         await SendCodeToClient(targetClient, code);
    //     }
    //     else
    //     {
    //         Console.WriteLine($"Client with MAC address {macAddress} not found.");
    //     }
    // }
    //
   
    
    // private async Task SendCodeToClient(TcpClient client, int code)
    // {
    //     try
    //     {
    //         NetworkStream clientStream = client.GetStream();
    //
    //         // Convert the code to bytes and send it to the client
    //         byte[] codeBytes = Encoding.ASCII.GetBytes(code.ToString());
    //         await clientStream.WriteAsync(codeBytes, 0, codeBytes.Length);
    //         clientStream.Flush();
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error sending code to client: {ex.Message}");
    //     }
    // }
    //
    
    private async void HandleClientComm(object client)
    {
        TcpClient tcpClient = (TcpClient)client;
        NetworkStream clientStream = tcpClient.GetStream();
       

        byte[] message = new byte[4096];

        while (true)
        {
            var bytesRead = 0;

            try
            {
                bytesRead = clientStream.Read(message, 0, 4096);
                // Console.WriteLine("bytes read "+ bytesRead);
            }
            catch
            {
                break;
            }
            
            if (bytesRead == 0)
            {
                break;
            }
            string data = Encoding.ASCII.GetString(message, 0, bytesRead);
            Console.WriteLine("Data: " + data);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine("TImestamp:" + timestamp);

            // combining data and timestamps into one string 
            string arduinoDataString = $"{timestamp}, received data: {data}";
            
            // converting arduino message to domain object - Plant Data 
            PlantData newData = MessageConverter.CreatePlantDataFromString(arduinoDataString);
            //sending a http request to backend api to save the data 
            await plantHttpClient.SavePlantDataAsync(newData);
            
            Console.WriteLine(arduinoDataString);
            
            //Code for sending responses to the Arduino
            // try
            // {
            //     // Example: Send a response back to the client
            //     Random random = new Random();
            //     String responseMessage = random.Next(10).ToString(); // Generates 0, 1, 2, 3, 4
            //     byte[] responseBytes = Encoding.ASCII.GetBytes(responseMessage);
            //     clientStream.Write(responseBytes, 0, responseBytes.Length);
            //     clientStream.Flush();
            //     Console.WriteLine("Data sent to Arduino");
            // }
            // catch
            // {
            //     Console.WriteLine("Error sending data to Arduino");
            //     break;
            // }
        }

        tcpClient.Close();
    }


    public static void Main()
    {
        TCPServer server = new TCPServer();
        Console.WriteLine("Server started. Listening for incoming connections...");
    }

}