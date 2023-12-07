using IoTBridge.Core.Base;

namespace IoTBridge.Core.JsonCaster
{
    public class SerializationResult : AResult
    {
        public string SerializedObject { get; set; }
    }
}