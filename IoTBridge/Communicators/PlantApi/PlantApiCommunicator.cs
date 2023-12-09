using IoTBridge.Communicators.Base;
using IoTBridge.Communicators.PlantApi.DTOs.Requests;
using IoTBridge.Communicators.PlantApi.DTOs.Responses;
using IoTBridge.Communicators.PlantApi.DTOs.Responses.Results;
using IoTBridge.Core.JsonCaster;

//TODO: Change the endpoints to the correct ones
namespace IoTBridge.Communicators.PlantApi
{
    public class PlantApiCommunicator : AHttpCommunicator, IPlantApiCommunicator
    {
        public PlantApiCommunicator(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<EmptyCommunicatorResult> RegisterDevice(int deviceId)
        {
            try
            {
                var parameters = new Dictionary<string, string>()
                {
                    { "deviceId", deviceId.ToString() }
                };
                await PostAsync(parameters, "/api/v1/devices");
                return new EmptyCommunicatorResult();
            }
            catch (Exception e)
            {
                return new EmptyCommunicatorResult()
                {
                    Error = e.Message
                };
            }
        }

        public async Task<EmptyCommunicatorResult> UpdateDeviceStatus(UpdateDeviceStatus updateDeviceStatus)
        {
            SerializationResult body = JsonCasterHelper.SerializeData(updateDeviceStatus);
            if (body.HasError)
            {
                return new EmptyCommunicatorResult()
                {
                    Error = body.Error
                };
            }

            try
            {
                await UpdateAsync(body.SerializedObject, "/api/v1/devices");
                return new EmptyCommunicatorResult();
            }
            catch (Exception e)
            {
                return new EmptyCommunicatorResult()
                {
                    Error = e.Message
                };
            }
        }

        public async Task<ExistingDeviceIdsResult> GetExistingDeviceIds()
        {
            try
            {
                string existingDeviceIdsContent = await GetAsync("/api/v1/devices");
                ExistingDeviceIdsResult deserializedDeviceIds = JsonCasterHelper.DeserializeData<ExistingDeviceIdsResult ,ExistingDeviceIds>(existingDeviceIdsContent);

                if (deserializedDeviceIds.HasError)
                {
                    return new ExistingDeviceIdsResult()
                    {
                        Error = deserializedDeviceIds.Error
                    };
                }
                return deserializedDeviceIds;
            }
            catch(Exception e)
            {
                return new ExistingDeviceIdsResult()
                {
                    Error = e.Message
                };
            }
        }

        public async Task<EmptyCommunicatorResult> SendPlantData(PlantDataRequest plantDataRequest)
        {
            SerializationResult body = JsonCasterHelper.SerializeData(plantDataRequest);
            if (body.HasError)
            {
                return new EmptyCommunicatorResult()
                {
                    Error = body.Error
                };
            }
            
            try
            {
                await PostAsync(body.SerializedObject, "/api/v1/devices");
                return new EmptyCommunicatorResult();
            }
            catch (Exception e)
            {
                return new EmptyCommunicatorResult()
                {
                    Error = e.Message
                };
            }
        }
    }
}