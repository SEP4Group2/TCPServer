using System.ComponentModel.DataAnnotations;

namespace Domain.Model;

public class PlantData
{

    public float? Humidity { get; set; }

    public float? Temperature { get; set; }
    
    public float UVLight { get; set; }
    
    public float Moisture { get; set; }
    
    [Key]
    public string TimeStamp { get; set; }

    public float TankLevel { get; set; }
}