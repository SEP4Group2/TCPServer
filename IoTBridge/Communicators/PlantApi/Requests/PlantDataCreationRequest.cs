using IoTBridge.Shared.OutgoingData;

namespace IoTBridge.Communicators.PlantApi.Requests
{
    public class PlantDataCreationRequest
    {
        public List<PlantDataApiDTO> PlantDataApi { get; set; }
    }
}