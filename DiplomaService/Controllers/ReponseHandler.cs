using Newtonsoft.Json;

namespace DiplomaService.Controllers;

public class ResponseHandler : IResponseHandler
{
    public void AddHeader(HttpResponse? response, string header, object metaData)
    {
        response?.Headers.Add(header, JsonConvert.SerializeObject(metaData));
    }
}