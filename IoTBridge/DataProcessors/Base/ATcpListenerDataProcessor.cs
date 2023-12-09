using System.Net.Sockets;
using IoTBridge.Core.Data;
using IoTBridge.Core.DataProcessor;

namespace IoTBridge.DataProcessors.Base;

public abstract class ATcpListenerDataProcessor<TRecievedData> : ADataProcessor<TRecievedData> 
    where TRecievedData : IRecievedData
{
    public virtual void HandleData(TcpClient tcpClient, TRecievedData data)
    {
        ProcessData(data);
    }
}