namespace IoTBridge.Core.Base
{
    public abstract class AResult
    {
        public string Error { get; set; }
        public bool HasError => !string.IsNullOrWhiteSpace(Error);
    }
}