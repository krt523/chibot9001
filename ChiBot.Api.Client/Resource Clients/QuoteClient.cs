using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChiBot.Domain.Models;

namespace ChiBot.Api.Client.ResourceClients
{
    public class QuoteClient : ResourceClient<Quote>
    {

        internal QuoteClient(HttpClient client, string resource) : base(client, resource)
        {
        }

        public async Task<Quote> GetRandomQuoteForPersonalityAsync(Personality personality)
        {
            HttpResponseMessage response = await _client.GetAsync($"Personality/{personality.Id}/{_resource}?random=true");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Quote>();
        }

        public async Task<Quote> GetSpecificQuoteForPersonalityAsync(Personality personality, int quoteNumber)
        {
            HttpResponseMessage response = await _client.GetAsync($"Personality/{personality.Id}/{_resource}?index=quoteNumber");
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Quote>();
        }

        public async Task<Quote> AddQuoteForPersonalityAsync(Personality personality, string quoteToAdd)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync<dynamic>($"Personality/{personality.Id}/{_resource}", new { content = quoteToAdd });
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Quote>();
        }
    }
}
