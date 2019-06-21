using System.Net.Http;


namespace ChiBot.Api.Client
{
    public interface IChiBotHttpClientFacotry
    {
        HttpClient CreateClient();
    }
}
