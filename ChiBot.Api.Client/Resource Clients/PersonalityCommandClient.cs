using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChiBot.Domain.Models;

namespace ChiBot.Api.Client.ResourceClients
{
    public class PersonalityCommandClient : ResourceClient<PersonalityCommand>
    {
        internal PersonalityCommandClient(HttpClient client, string resource): base(client, resource)
        {

        }

        public async Task<PersonalityCommand> GetCommandForPersonalityAsync(Personality personality, string trigger)
        {
            HttpResponseMessage response = await _client.GetAsync($"Personality/{personality.Id}/{_resource}/{trigger}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<PersonalityCommand>();
        }

        public async Task<List<PersonalityCommand>> GetCommandsForPersonalityAsync(Personality personality)
        {
            HttpResponseMessage response = await _client.GetAsync($"Personality/{personality.Id}/{_resource}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<List<PersonalityCommand>>();
        }

        public async Task<PersonalityCommand> AddCommandForPersonalityAsync(Personality personality, PersonalityCommand command)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync($"Personality/{personality.Id}/{_resource}", command);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<PersonalityCommand>();
        }

        public async Task UpdateCommandForPersonalityAsync(Personality personality, string trigger, string commandResponse)
        {
            HttpResponseMessage response = await _client.PutAsJsonAsync($"Personality/{personality.Id}/{_resource}/{trigger}", new { response = commandResponse });
            response.EnsureSuccessStatusCode();
            return;
        }

        public async Task DeleteCommandForPersonalityAsync(Personality personality, string trigger)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"Personality/{personality.Id}/{_resource}/{trigger}");
            response.EnsureSuccessStatusCode();
            return;
        }
    }
}
