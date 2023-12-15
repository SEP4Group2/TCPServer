using IoTBridge.DataCaching.Base;
using IoTBridge.Shared.OutgoingData;

namespace IoTBridge.DataCaching
{
    public class PlantDataApiCache : IPlantDataApiCache
    {
        private readonly int memoryProtectionMaxCache;
        private Dictionary<int, List<PlantDataApiDTO>> plantDataByConnectionId = new();

        public PlantDataApiCache(int memoryProtectionMaxCache)
        {
            this.memoryProtectionMaxCache = memoryProtectionMaxCache;
        }

        public void CachePlantData(int connectionId, PlantDataApiDTO dataRequest)
        {
            if (!plantDataByConnectionId.ContainsKey(connectionId))
            {
                plantDataByConnectionId.Add(connectionId, new List<PlantDataApiDTO>(){dataRequest});
            }
            else
            {
                if (HasConnectionReachedMaxCache(connectionId, memoryProtectionMaxCache))
                {
                    Console.WriteLine($"Maximum cache reached for connection with id: {connectionId}");
                    plantDataByConnectionId[connectionId].Clear();
                }
                plantDataByConnectionId[connectionId].Add(dataRequest);           
                Console.WriteLine($"Data for connection: {connectionId} successfully cached");
            }
        }

        public bool HasConnectionReachedMaxCache(int connectionId, int maxCachedData)
        {
            return plantDataByConnectionId.ContainsKey(connectionId) && plantDataByConnectionId[connectionId].Count >= maxCachedData;
        }
    
        public List<PlantDataApiDTO> GetCachedDataByConnectionId(int connectionId)
        {
            return plantDataByConnectionId[connectionId];
        }
    
        public void ClearCacheByConnectionId(int connectionId)
        {
            plantDataByConnectionId.Remove(connectionId);
        }
    }
}