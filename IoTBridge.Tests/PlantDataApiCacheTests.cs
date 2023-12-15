using IoTBridge.DataCaching;
using IoTBridge.Shared.OutgoingData;

namespace IoTBridge.Tests;

public class PlantDataApiCacheTests
{
    private PlantDataApiCache cache;
    private readonly int maxCacheSize = 10;

    [SetUp]
    public void SetUp()
    {
        cache = new PlantDataApiCache(maxCacheSize);
    }

    [Test]
    public void PlantDataApiCache_Should_Cache_Data()
    {
        int connectionId = 1;
        var dataRequest = new PlantDataApiDTO()
        {
            Humidity = 10.0f,
            Temperature = 12.0f,
            UVLight = 10.0f,
            Moisture = 50.0f,
            TankLevel = 10.0f
        };

        cache.CachePlantData(connectionId, dataRequest);

        List<PlantDataApiDTO> cachedData = cache.GetCachedDataByConnectionId(connectionId);
        Assert.That(cachedData.First(), Is.EqualTo(dataRequest));
    }

    [Test]
    public void PlantDataApiCache_Should_Reach_Max_Cache()
    {
        int connectionId = 1;
        
        for (int i = 0; i < maxCacheSize; i++)
        {
            cache.CachePlantData(connectionId, new PlantDataApiDTO());
        }

        Assert.True(cache.HasConnectionReachedMaxCache(connectionId, maxCacheSize));
    }

    [Test]
    public void PlantDataApiCache_Should_RemoveData()
    {
        int connectionId = 1;
        cache.CachePlantData(connectionId, new PlantDataApiDTO());

        cache.ClearCacheByConnectionId(connectionId);

        Assert.Throws<KeyNotFoundException>(() => cache.GetCachedDataByConnectionId(connectionId));
    }
}