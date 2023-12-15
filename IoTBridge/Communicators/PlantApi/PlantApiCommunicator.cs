using IoTBridge.Communicators.Base;
using IoTBridge.Communicators.PlantApi.Base;
using IoTBridge.Communicators.PlantApi.Requests;
using IoTBridge.Communicators.PlantApi.Results;
using IoTBridge.Communicators.PlantApi.Results.Data;
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
                await PostAsync($"device/registerDevice/{deviceId}");
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

        public async Task<EmptyCommunicatorResult> UpdateDeviceStatus(UpdateDeviceStatusRequest updateDeviceStatusRequest)
        {
            SerializationResult body = JsonCasterHelper.SerializeData(updateDeviceStatusRequest);
            if (body.HasError)
            {
                return new EmptyCommunicatorResult()
                {
                    Error = body.Error
                };
            }

            try
            {
                await UpdateAsync(body.SerializedObject, "device/changeStatusCode");
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
                string existingDeviceIdsContent = await GetAsync("device/getAllIds");
                Console.WriteLine($"HttpCommunicator recieved this data: {existingDeviceIdsContent}");
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

        public async Task<EmptyCommunicatorResult> SendPlantData(PlantDataCreationRequest plantDataCreationRequest)
        {
            SerializationResult body = JsonCasterHelper.SerializeData(plantDataCreationRequest);
            if (body.HasError)
            {
                return new EmptyCommunicatorResult()
                {
                    Error = body.Error
                };
            }
            
            try
            {
                await PostAsync(body.SerializedObject, "plantData/savePlantData");
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