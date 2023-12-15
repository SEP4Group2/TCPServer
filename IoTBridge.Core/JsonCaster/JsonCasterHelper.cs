namespace IoTBridge.Core.JsonCaster
{
    public static class JsonCasterHelper
    {
        public static TDeserializedResult DeserializeData<TDeserializedResult, TDeserializedObject>(string data)
            where TDeserializedResult : ADeserializationResult<TDeserializedObject>, new()
            where TDeserializedObject : new()
        {
            try
            {
                var deserializedData = Newtonsoft.Json.JsonConvert.DeserializeObject<TDeserializedObject>(data);
                if (deserializedData == null)
                {
                    return new TDeserializedResult()
                    {
                        Error = "Failed to deserialize data. The deserializedData is NULL"
                    };
                }

                return new TDeserializedResult()
                {
                    Data = deserializedData
                };
            }
            catch (Exception e)
            {
                return new TDeserializedResult()
                {
                    Error = e.Message
                };
            }
        }

        public static SerializationResult SerializeData<TObject>(TObject data)
        {
            try
            {
                string? serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                if (string.IsNullOrWhiteSpace(serializedData))
                {
                    return new SerializationResult()
                    {
                        Error = "Failed to deserialize data. The deserializedData is NULL"
                    };
                }

                return new SerializationResult()
                {
                    SerializedObject = serializedData
                };
            }
            catch (Exception e)
            {
                return new SerializationResult()
                {
                    Error = e.Message
                };
            }
        }
    }
}