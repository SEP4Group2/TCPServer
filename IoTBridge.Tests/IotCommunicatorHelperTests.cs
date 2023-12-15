using IoTBridge.Communicators.Iot.Base;
using IoTBridge.Communicators.Iot.Data;

namespace IoTBridge.Tests;

public class IotCommunicatorHelperTests
{
    
    [Test]
    public void IotCommunicatorHelper_Should_Convert_Ids_Correctly()
    {
        int firstId = 1;
        int secondId  = 15;

        string firstConvertedId = IotCommunicatorHelper.ConvertRegistrationIdToMessage(firstId);
        string secondConvertedId = IotCommunicatorHelper.ConvertRegistrationIdToMessage(secondId);
        
        Assert.That(firstConvertedId, Is.EqualTo("99999901"));
        Assert.That(secondConvertedId, Is.EqualTo("99999915"));
    }
    
    
    [Test]
    public void IotCommunicatorHelper_Should_Convert_Actions_Correctly()
    {
        var firstAction = IotActions.PUMP;
        var secondAction = IotActions.DATA;
        var thirdAction = IotActions.LED;

        string firstConvertedAction = IotCommunicatorHelper.ConvertActionToMessage(firstAction);
        string secondConvertedAction = IotCommunicatorHelper.ConvertActionToMessage(secondAction);
        string thirdConvertedAction = IotCommunicatorHelper.ConvertActionToMessage(thirdAction);
        
        Assert.That(firstConvertedAction, Is.EqualTo("16161601"));
        Assert.That(secondConvertedAction, Is.EqualTo("16161602"));
        Assert.That(thirdConvertedAction, Is.EqualTo("16161603"));
    }
    
    [Test]
    public void IotCommunicatorHelper_Should_Convert_Message_Correctly()
    {
        string message = "ID24";
        string expectedConvertedMessage = "01130204";

        string convertedMessage = IotCommunicatorHelper.ConvertStringToMessage(message);
        
        Assert.That(expectedConvertedMessage, Is.EqualTo(convertedMessage));
    }
    
}