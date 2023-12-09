using System.Text;
using IoTBridge.Connection.Base;

namespace IoTBridge.Communicators.Base;

public abstract class ATcpCommunicator
{
    private readonly ITcpConnectionService tcpConnectionService;
    public ATcpCommunicator(ITcpConnectionService tcpConnectionService)
    {
        this.tcpConnectionService = tcpConnectionService;
    }
    protected void Send(int deviceId, string message)
    {
        message += "\n";
        byte[] data = ConvertStringToByteArray(message);
        try
        {
            tcpConnectionService.SendData(deviceId, data);
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occured when sending data to device: {deviceId}");
            Console.WriteLine($"Error: {e}");
        }
    }
    
    private byte[] ConvertStringToByteArray(string input)
    {
        return Encoding.ASCII.GetBytes(input);
    }
}