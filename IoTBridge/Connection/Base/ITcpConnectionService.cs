using System.Net.Sockets;
using IoTBridge.Core.Connection;

namespace IoTBridge.Connection.Base;

public interface ITcpConnectionService : IConnectionService<ITcpConnection, TcpClient>
{
    List<int> GetExistingIds();
    void SetExistingIds(List<int> existingIds);
    int GetConnectionIdByTcpClient(TcpClient client);
    bool ConnectionExists(int deviceId);
}