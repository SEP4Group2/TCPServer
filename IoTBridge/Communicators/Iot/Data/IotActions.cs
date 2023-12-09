namespace IoTBridge.Communicators.Iot.Data;

public enum IotActions
{
    DEFAULT,
    PUMP, // Command to start watering the plant
    DATA, // Command to start sending the data
    LED, // Command to lightup a led
}