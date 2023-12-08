namespace IoTBridge.Connection.Base;

public static class NewIdGenerator
{
    public static int GenerateNewId(List<int> alreadyExistingIds)
    {
        Random random = new Random();

        int newId = random.Next(10, 100);

        while (alreadyExistingIds.Contains(newId))
        {
            newId = random.Next(10, 100);
        }
        
        return newId;
    }
}