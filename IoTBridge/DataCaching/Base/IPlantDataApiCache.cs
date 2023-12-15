using IoTBridge.Shared.OutgoingData;

namespace IoTBridge.DataCaching.Base
{
    public interface IPlantDataApiCache
    {
        void CachePlantData(int connectionId, PlantDataApiDTO dataRequest);
        bool HasConnectionReachedMaxCache(int connectionId, int maxCachedData);
        void ClearCacheByConnectionId(int connectionId);
        List<PlantDataApiDTO> GetCachedDataByConnectionId(int connectionId);
    }
}