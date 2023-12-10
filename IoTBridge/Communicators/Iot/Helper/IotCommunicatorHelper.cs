using System.Text;
using IoTBridge.Communicators.Iot.Data;

namespace IoTBridge.Communicators.Iot.Helper;

public static class IotCommunicatorHelper
{
    public static string ConvertRegistrationIdToMessage(int id)
    {
        if (id < 10)
        {
            string stringId = "0" + id;
            return "999999" + stringId;
        }
        return "999999" + id;
    }
    
    public static string ConvertActionToMessage(IotActions action)
    {
        switch (action)
        {
            case IotActions.PUMP:
                return "16161601";
            case IotActions.DATA:
                return "16161602";
            case IotActions.LED:
                return "16161603";
            default:
                return "16161616";
        }
    }
    
    public static string ConvertStringToMessage(string text)
    {
        if (text.Length != 4)
        {
            throw new ArgumentException("Input string must be exactly 4 characters long.");
        }

        StringBuilder result = new StringBuilder();
        foreach (char c in text)
        {
            try
            {
                int index = CharToIndex(c);
                result.Append(index.ToString("00"));
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error when converting string to message: {e}");
                return string.Empty;
            }
        }

        return result.ToString();
    }
    
    private static int CharToIndex(char c)
    {
        switch (c)
        {
            case '0': return 00;
            case '1': return 01;
            case '2': return 02;
            case '3': return 03;
            case '4': return 04;
            case '5': return 05;
            case '6': return 06;
            case '7': return 07;
            case '8': return 08;
            case '9': return 09;
            case 'A': return 10;
            case 'B': return 11;
            case 'C': return 12;
            case 'D': return 13;
            case 'E': return 14;
            case 'F': return 15;
            case 'G': return 06;
            case 'O': return 00;
            case '-': return 16;
            case 'I': return 01;
            case ' ': return 17;
            default: throw new ArgumentException($"Invalid character: {c}");
        }
    }
}