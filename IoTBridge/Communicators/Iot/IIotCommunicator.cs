using IoTBridge.Communicators.Iot.Data;

namespace IoTBridge.Communicators.Iot;

public interface IIotCommunicator
{
    void SendRegistrationId(int deviceId);
    void SendAction(int deviceId, IotActions action);
}