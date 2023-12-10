using System.ComponentModel.DataAnnotations;

namespace IoTBridge.Communicators.PlantApi.DTOs.Requests;

public class PlantDataCreationDTO
{
    public int DeviceId { get; set; }
    public float Humidity { get; set; }
    public float Temperature { get; set; }
    public float UVLight { get; set; }
    public float Moisture { get; set; }
    public string TimeStamp { get; set; }
    public float TankLevel { get; set; } 
}

public class PlantDataCreationListDTO
{
    public List<PlantDataCreationDTO> PlantDataApi { get; set; }
}