using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChiBot.Domain.Models;
using ChiBot.Api.Client;

namespace ChiBot.TwitchClient
{
    /// <summary>
    /// Simple ChannelBroker. At this point in development only one Twitch Client will be running. This ChannelBroker returns all channels in the database when it is asked for channel assigments. 
    /// Since the back end systems are not track channel assignments, it does nothing when asked to release channels.
    /// </summary>
    class ChannelBroker : IChannelBroker
    {
        private ChiBotApiClient _chiBotApiClient;

        public ChannelBroker(ChiBotApiClient chiBotApiClient)
        {
            _chiBotApiClient = chiBotApiClient;
        }

        public void ReleaseChannelAssignments(string clientId)
        {
            return;
        }

        public Task ReleaseChannelAssignmentsAsync(string clientId)
        {
            return Task.CompletedTask;
        }

        public List<TwitchChannel> RequestChannelAssignments(string clientId)
        {
            return RequestChannelAssignmentsAsync(clientId).Result;
        }

        public Task<List<TwitchChannel>> RequestChannelAssignmentsAsync(string clientId)
        {
            return _chiBotApiClient.TwitchChannels.GetAllAsync();
        }
    }
}
