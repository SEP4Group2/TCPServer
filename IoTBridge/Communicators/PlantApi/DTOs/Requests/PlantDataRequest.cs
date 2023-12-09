using System.ComponentModel.DataAnnotations;

namespace IoTBridge.Communicators.PlantApi.DTOs.Requests;

public class PlantDataApi
{
    public int DeviceId { get; set; }
    public float? Humidity { get; set; }

    public float? Temperature { get; set; }
    
    public float UVLight { get; set; }
    
    public float Moisture { get; set; }
    
    [Key]
    public string TimeStamp { get; set; }

    public float TankLevel { get; set; } 
}

public class PlantDataRequest
{
    public List<PlantDataApi> PlantDataApi { get; set; }
}