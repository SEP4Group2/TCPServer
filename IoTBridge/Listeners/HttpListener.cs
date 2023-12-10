using System.Net;
using IoTBridge.Core.JsonCaster;
using IoTBridge.DataProcessors.PlantApi.Base;
using IoTBridge.DataProcessors.PlantApi.Data;
using IoTBridge.Listeners.Base;
using IoTBridge.Listeners.Results;

namespace IoTBridge.Listeners;

public class HttpListener : IHttpListener
{
    public Action<IHttpReceivedData> OnMessageReceived { get; set; }
    
    private readonly string listenerUrl;
    private readonly string endpoint;
    
    private System.Net.HttpListener httpListener;

    public HttpListener(string listenerUrl, string endpoint)
    {
        this.listenerUrl = listenerUrl;
        this.endpoint = endpoint;
    }
    
    public void Initialize()
    {
        httpListener = new System.Net.HttpListener();
        httpListener.Prefixes.Add(listenerUrl);
    }

    public void Run()
    {
        httpListener.Start();
        Console.WriteLine("HttpListener is up and running");
        while (true)
        {
            Console.WriteLine("HttpListener is waiting for client");
            HttpListenerContext context = httpListener.GetContext();
            Console.WriteLine("New client connected to an HttpListner");
            
            Thread clientThread = new Thread(() => HandleClientsMessages(context));
            clientThread.Start();
        }
    }

    private void HandleClientsMessages(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        
        if (!request.Url.AbsolutePath.Equals(endpoint))
        {
            Console.WriteLine($"Client tried to access an endpoint that is not defined: {request.Url.AbsolutePath}");
            Console.WriteLine($"The available endpoint is: {endpoint}");
            CloseConnection(response, 404);
            return;
        }

        var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
        string data = reader.ReadToEnd();
        
        CreatePlantApiRecievedDataAResult createPlantApiRecievedDataAResult = JsonCasterHelper.DeserializeData<CreatePlantApiRecievedDataAResult, HttpReceivedData>(data);
        if (createPlantApiRecievedDataAResult.HasError)
        {
            Console.WriteLine(createPlantApiRecievedDataAResult.Error);
            reader.Close();
            CloseConnection(response, 400);
            return;
        }
        
        OnMessageReceived.Invoke(createPlantApiRecievedDataAResult.Data);
        reader.Close();
        // Maybe move this inside of the OnMessageRecieved invokation
        CloseConnection(response, 200);
    }

    private void CloseConnection(HttpListenerResponse response, int statusCode)
    {
        response.StatusCode = statusCode;
        response.Close();
    }
}