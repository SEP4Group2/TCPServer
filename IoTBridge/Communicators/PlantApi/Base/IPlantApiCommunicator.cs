using IoTBridge.Communicators.PlantApi.Requests;
using IoTBridge.Communicators.PlantApi.Results;

namespace IoTBridge.Communicators.PlantApi.Base
{
    public interface IPlantApiCommunicator
    {
        Task<EmptyCommunicatorResult> RegisterDevice(int deviceId);
        Task<EmptyCommunicatorResult> UpdateDeviceStatus(UpdateDeviceStatusRequest updateDeviceStatusRequest);
        Task<ExistingDeviceIdsResult> GetExistingDeviceIds();
        Task<EmptyCommunicatorResult> SendPlantData(PlantDataCreationRequest plantDataCreationRequest);
    }
}