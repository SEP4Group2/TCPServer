using IoTBridge.Core.Base;

namespace IoTBridge.Core.JsonCaster;

public abstract class ADeserializationResult<TDeserializedObject> : AResult
{
    public TDeserializedObject Data { get; set; }
}