using System;
using IoTBridge.Core.Data;

namespace IoTBridge.Core.Listener
{
    public interface IListener
    {
        void Initialize();
        void Run();
    }
}