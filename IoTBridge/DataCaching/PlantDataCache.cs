using IoTBridge.IncomingData.Iot;

namespace IoTBridge.DataCaching;

public class PlantDataCache : IPlantDataCache
{
    private readonly int memoryProtectionMaxCache;
    private Dictionary<int, List<PlantData>> plantDataByConnectionId = new();

    public PlantDataCache(int memoryProtectionMaxCache)
    {
        this.memoryProtectionMaxCache = memoryProtectionMaxCache;
    }

    public void CachePlantData(int connectionId, PlantData dataRequest)
    {
        if (!plantDataByConnectionId.ContainsKey(connectionId))
        {
            plantDataByConnectionId.Add(connectionId, new List<PlantData>(){dataRequest});
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
    
    public List<PlantData> GetCachedDataByConnectionId(int connectionId)
    {
        return plantDataByConnectionId[connectionId];
    }
    
    public bool ClearCacheByConnectionId(int connectionId)
    {
        return plantDataByConnectionId.Remove(connectionId);
    }
}