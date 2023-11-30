using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
namespace TCPServer.util;
[ApiController]
[Route("[controller]")]
public class ArduinoController:ControllerBase
{   
    private TCPServer tcpServer;

    
    public ArduinoController()
    {
        // Initialize the TCPServer instance
        tcpServer = new TCPServer();
    }
    
    [HttpPost]
    [Route("WaterPumpCode")]
    public async Task<ActionResult> SendSignalToArduino()
    {
        try
        {
            TcpClient tcpClient = tcpServer.GetConnectedClient();

            if (tcpClient != null)
            {
                // Send the signal "1" to the Arduino with the specific TcpClient
                await tcpServer.sendCodeForWaterPump(tcpClient);

                // You can return a success response if needed
                return Ok("Signal sent to Arduino");
            }

            // You can return a success response if needed
            return Ok("Signal sent to Arduino");
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}
