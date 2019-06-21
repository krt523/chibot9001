using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChiBot.Domain.Models;


namespace ChiBot.Api.Client.ResourceClients
{
    public class PersonalityClient : ResourceClient<Personality>
    {
        internal PersonalityClient(HttpClient client, string resource) : base(client, resource)
        {
        }
    }
}
