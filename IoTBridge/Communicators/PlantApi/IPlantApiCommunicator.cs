using IoTBridge.Communicators.Base;
using IoTBridge.Communicators.PlantApi.DTOs.Requests;
using IoTBridge.Communicators.PlantApi.DTOs.Responses.Results;

namespace IoTBridge.Communicators.PlantApi;

public interface IPlantApiCommunicator
{
    Task<EmptyCommunicatorResult> RegisterDevice(int deviceId);
    Task<EmptyCommunicatorResult> UpdateDeviceStatus(UpdateDeviceStatus updateDeviceStatus);
    Task<ExistingDeviceIdsResult> GetExistingDeviceIds();
    Task<EmptyCommunicatorResult> SendPlantData(PlantDataCreationListDTO plantDataCreationListDto);
}