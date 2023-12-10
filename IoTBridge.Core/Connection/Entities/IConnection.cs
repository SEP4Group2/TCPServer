using System;

namespace IoTBridge.Core.Connection.Entities
{
    public interface IConnection<TClient> where TClient : IDisposable, new()
    {
        TClient Client { get; set; }
    }
}