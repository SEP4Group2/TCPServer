using IoTBridge.Communicators.PlantApi.DTOs.Requests;

namespace IoTBridge.DataCaching
{
    public interface IPlantDataApiCache
    {
        void CachePlantData(int connectionId, PlantDataCreationDTO dataRequest);
        bool HasConnectionReachedMaxCache(int connectionId, int maxCachedData);
        bool ClearCacheByConnectionId(int connectionId);
        List<PlantDataCreationDTO> GetCachedDataByConnectionId(int connectionId);
    }
}