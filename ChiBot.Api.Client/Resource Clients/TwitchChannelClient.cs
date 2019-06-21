using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChiBot.Domain.Models;

namespace ChiBot.Api.Client.ResourceClients
{
    public class TwitchChannelClient : ResourceClient<TwitchChannel>
    {
        internal TwitchChannelClient(HttpClient client, string resource) : base(client, resource) 
        {
        }
    }
}
