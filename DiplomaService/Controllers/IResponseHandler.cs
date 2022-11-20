namespace DiplomaService.Controllers;

public interface IResponseHandler
{
    public void AddHeader(HttpResponse? response, string header, object metaData);
}