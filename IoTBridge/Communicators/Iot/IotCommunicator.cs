using IoTBridge.Communicators.Base;
using IoTBridge.Communicators.Iot.Data;
using IoTBridge.Communicators.Iot.Helper;
using IoTBridge.Connection.Base;

namespace IoTBridge.Communicators.Iot;

public class IotCommunicator : ATcpCommunicator,  IIotCommunicator
{
    public IotCommunicator(ITcpConnectionService tcpConnectionService) : base(tcpConnectionService)
    {
    }

    public void SendRegistrationId(int deviceId)
    {
        string convertedId = IotCommunicatorHelper.ConvertRegistrationIdToMessage(deviceId);
        Send(deviceId, convertedId);
    }

    public void SendAction(int deviceId, IotActions action)
    {
        string convertedAction = IotCommunicatorHelper.ConvertActionToMessage(action);
        Send(deviceId, convertedAction);
    }
}