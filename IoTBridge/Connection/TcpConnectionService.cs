using System.Net.Sockets;
using IoTBridge.Connection.Base;
using IoTBridge.Connection.Base.Data;

namespace IoTBridge.Connection
{
    public class TcpConnectionService : ITcpConnectionService
    {
        private Dictionary<int, ITcpConnection> connections = new Dictionary<int, ITcpConnection>();
        private List<int> existingIds = new List<int>();
        
        public void AddConnection(ITcpConnection connection)
        {
            Console.WriteLine("Cached the connection: " + connection.ConnectionId);
            if (connections.ContainsKey(connection.ConnectionId))
            {
                connections[connection.ConnectionId] = connection;
                return;
            }
            connections.Add(connection.ConnectionId, connection);
            
            if (existingIds.Contains(connection.ConnectionId))
            {
                return;
            }
            existingIds.Add(connection.ConnectionId);
            Console.WriteLine("Addded the connection to existingIds: " + connection.ConnectionId);
        }

        public void CloseAndRemoveConnection(int connectionId)
        {
            connections[connectionId].Client.Close();
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
            Console.WriteLine("Sending the data........");
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        public List<int> GetExistingIds()
        {
            return existingIds;
        }

        public void SetExistingIds(List<int> existingIds)
        {
            this.existingIds = existingIds;
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