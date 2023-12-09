namespace IoTBridge.IncomingData.Iot
{
    public class PlantData
    {
        public int DeviceId { get; set; }
        public float Humidity { get; set; }
        public float Temperature { get; set; }
        public float UVLight { get; set; }
        public float Moisture { get; set; }
        public float TankLevel { get; set; }
    }
}