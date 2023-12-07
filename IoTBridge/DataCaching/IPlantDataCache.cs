using IoTBridge.IncomingData.Iot;

namespace IoTBridge.DataCaching
{
    public interface IPlantDataCache
    {
        void CachePlantData(int connectionId, PlantData dataRequest);
        bool HasConnectionReachedMaxCache(int connectionId, int maxCachedData);
        bool ClearCacheByConnectionId(int connectionId);
        List<PlantData> GetCachedDataByConnectionId(int connectionId);
    }
}