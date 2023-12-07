using IoTBridge.Core.Data;
using IoTBridge.Core.DataProcessor;

namespace IoTBridge.DataProcessors.Base
{
    public abstract class AHttpListenerDataProcessor<TRecievedData> : ADataProcessor<TRecievedData> 
        where TRecievedData : IRecievedData
    {
        public virtual void HandleData(TRecievedData data)
        {
            ProcessData(data);
        }
    }
}