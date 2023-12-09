using System.Net.Sockets;
using IoTBridge.IncomingData.Iot;

namespace IoTBridge.DataProcessors.Iot.Base;

public interface IIotDataProcessorService
{
    void RegisterDevice(TcpClient client, RegistrationData registrationData , int invalidId);
    void ForwardPlantDataToCache(int connectionId, PlantData plantData);
    void UpdateDeviceStatus(int connectionId, bool status);
}