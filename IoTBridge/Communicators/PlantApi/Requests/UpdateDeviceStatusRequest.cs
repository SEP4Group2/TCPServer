namespace IoTBridge.Communicators.PlantApi.Requests
{
    public class UpdateDeviceStatusRequest
    {
        public bool Status { get; set; }
        public int DeviceId { get; set; }
    }
}