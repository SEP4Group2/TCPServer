using System;
using IoTBridge.Core.Connection.Entities;

namespace IoTBridge.Core.Connection
{
    public interface IConnectionService<TConnection, TClient>
        where TConnection : IConnection<TClient>
        where TClient : IDisposable, new()
    {
        void AddConnection(TConnection connection);
        void CloseAndRemoveConnection(int connectionId);
        void SendData(int connectionId, byte[] data);
    }
}