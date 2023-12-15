using IoTBridge.Communicators.Iot.Data;

namespace IoTBridge.Communicators.Iot.Base
{
    public interface IIotCommunicator
    {
        void SendRegistrationId(int deviceId);
        void SendAction(int deviceId, IotActions action);
        void SendMessage(int deviceId, string message);
    }
}