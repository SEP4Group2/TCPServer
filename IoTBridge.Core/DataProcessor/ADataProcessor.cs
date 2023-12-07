using IoTBridge.Core.Data;

namespace IoTBridge.Core.DataProcessor
{
    public abstract class ADataProcessor<TRecievedData> 
        where TRecievedData : IRecievedData
    {
        protected abstract void ProcessData(TRecievedData data);
    }
}