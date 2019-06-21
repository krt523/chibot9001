using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using ChiBot.Api.Client.ResourceClients;

namespace ChiBot.Api.Client
{
    public class ChiBotApiClient
    {
        private HttpClient _client;
        public PersonalityClient Personalities { get; private set; }
        public TwitchChannelClient TwitchChannels { get; private set; }
        public QuoteClient Quotes { get; private set; }
        public PersonalityCommandClient PersonalityCommands { get; private set; }

        public ChiBotApiClient(IChiBotHttpClientFacotry httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            Personalities = new PersonalityClient(_client, "Personalities");
            TwitchChannels = new TwitchChannelClient(_client, "TwitchChannels");
            Quotes = new QuoteClient(_client, "Quotes");
            PersonalityCommands = new PersonalityCommandClient(_client, "PersonalityCommands");
        }
    }
}
