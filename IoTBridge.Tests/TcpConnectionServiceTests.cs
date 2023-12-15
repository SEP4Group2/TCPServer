using System.Net.Sockets;
using IoTBridge.Connection;
using IoTBridge.Connection.Base;
using IoTBridge.Connection.Base.Data;
using IoTBridge.Connection.Entities;
using IoTBridge.Shared.OutgoingData;

namespace IoTBridge.Tests;

public class TcpConnectionServiceTests
{
    private ITcpConnectionService tcpConnectionService;

    [SetUp]
    public void SetUp()
    {
        tcpConnectionService = new TcpConnectionService();
    }

    [Test]
    public void TcpConnectionService_Should_Add_Retrieve_Connection()
    {
        int connectionId = 1;
        var connection = new TcpConnection()
        {
            ConnectionId = connectionId,
            Client = new TcpClient()
        };

        tcpConnectionService.AddConnection(connection);

        ITcpConnection retrievedConnection = tcpConnectionService.GetConnection(connectionId);

        Assert.That(connection, Is.EqualTo(retrievedConnection));
    }
    
    [Test]
    public void TcpConnectionService_Should_Remove_Connection()
    {
        int connectionId = 1;
        var connection = new TcpConnection()
        {
            ConnectionId = connectionId,
            Client = new TcpClient()
        };

        tcpConnectionService.AddConnection(connection);
        tcpConnectionService.CloseAndRemoveConnection(connectionId);

        Assert.Throws<KeyNotFoundException>(() => tcpConnectionService.GetConnection(connectionId)); 
    }
    
    [Test]
    public void TcpConnectionService_Should_Add_Retrieve_ExistingIds()
    {
        int connectionId = 1;
        var existingIds = new List<int>()
        {
            1, 2, 3
        };

        tcpConnectionService.SetExistingIds(existingIds);

        List<int> retrievedIds = tcpConnectionService.GetExistingIds();

        for (var i = 0; i < retrievedIds.Count; i++)
        {
            Assert.True(existingIds[i] == retrievedIds[i]);
        }
    }
    
    [Test]
    public void TcpConnectionService_Should_Get_Connection_By_TcpClient()
    {
        int connectionId = 1;
        var tcpClient = new TcpClient();
        var connection = new TcpConnection()
        {
            ConnectionId = connectionId,
            Client = tcpClient
        };

        tcpConnectionService.AddConnection(connection);

        int retrievedId = tcpConnectionService.GetConnectionIdByTcpClient(tcpClient);

        Assert.True(retrievedId == connectionId);
    }
}