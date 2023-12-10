namespace IoTBridge.Communicators.PlantApi.DTOs.Requests;

public class UpdateDeviceStatus
{
    public bool Status { get; set; }
    public int DeviceId { get; set; }
}