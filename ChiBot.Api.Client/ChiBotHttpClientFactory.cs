using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;


namespace ChiBot.Api.Client
{
    public class ChiBotHttpClientFactory : IChiBotHttpClientFacotry
    {
        private string _baseApiUrl;
        private string _username;
        private string _password;

        public ChiBotHttpClientFactory(string baseUrl, string userName, string password)
        {
            _baseApiUrl = baseUrl;
            _username = userName;
            _password = password;
        }

        public HttpClient CreateClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_baseApiUrl)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string authParameter = $"{_username}:{_password}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(authParameter)));

            return client;
        }
    }
}
