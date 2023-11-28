using Microsoft.AspNetCore.Mvc;

namespace TCPServer.util;
[ApiController]
[Route("[controller]")]
public class ArduinoController:ControllerBase
{   
    private TCPServer tcpServer;

    [HttpPost]
    [Route("WaterPumpCode")]
    public async Task<ActionResult> SendSignalToArduino()
    {
        try
        {
            // Send the signal "1" to the Arduino
            // await SendSignalToArduinoTcp();

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
