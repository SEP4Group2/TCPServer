using System.Net.Sockets;
using IoTBridge.Connection.Base;

namespace IoTBridge.Connection
{
    public class TcpConnectionService : ITcpConnectionService
    {
        private Dictionary<int, ITcpConnection> connections = new Dictionary<int, ITcpConnection>();
        private List<int> existingIds = new List<int>();
        
        public void AddConnection(ITcpConnection connection)
        {
            Console.WriteLine("Cached the connection: " + connection.ConnectionId);
            connections.Add(connection.ConnectionId, connection);
            
            if (existingIds.Contains(connection.ConnectionId))
            {
                return;
            }
            existingIds.Add(connection.ConnectionId);
            Console.WriteLine("Addded the connection to existingIds: " + connection.ConnectionId);
        }

        public void RemoveConnection(int connectionId)
        {
            connections.Remove(connectionId);
        }

        public ITcpConnection GetConnection(int connectionId)
        {
            return connections[connectionId];
        }

        public void SendData(int connectionId, byte[] data)
        {
            ITcpConnection connection = GetConnection(connectionId);
            
            NetworkStream stream = connection.Client.GetStream();
            stream.Write(data, 0, data.Length);
        }

        public List<int> GetExistingIds()
        {
            return existingIds;
        }

        public void SetExistingIds(List<int> existingIds)
        {
            this.existingIds = existingIds;
        }

        public TcpClient GetTcpClient(int connectionId)
        {
            return connections[connectionId].Client;
        }

        public int GetConnectionIdByTcpClient(TcpClient client)
        {
            ITcpConnection? connection = connections.Values.FirstOrDefault(x => x.Client == client);
            if (connection == null)
            {
                return -1;
            }
            return connection.ConnectionId;
        }
    }
}