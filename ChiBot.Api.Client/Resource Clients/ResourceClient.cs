using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.Api.Client.ResourceClients
{
    public class ResourceClient<T>
    {
        protected readonly HttpClient _client;
        protected readonly string _resource;

        internal ResourceClient(HttpClient client, string resource)
        {
            _client = client;
            _resource = resource;
        }

        public async Task<List<T>> GetAllAsync()
        {
            HttpResponseMessage response = await _client.GetAsync($"{_resource}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<T>>();
        }

        public async Task<T> AddAsync(T resource)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"{_resource}", resource);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>();
        }

        public async Task UpdateAsync(T resource)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"{_resource}", resource);
            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{_resource}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<T>();
        }

        public async Task DeleteByIdAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"{_resource}/{id}");
            response.EnsureSuccessStatusCode();
            return;
        }
    }
}
