using ChiBot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.TwitchClient
{
    /// <summary>
    /// The ChannelBroker Interface exposes methods used for negotiating channel assignments if multiple twitch clients are running.
    /// </summary>
    public interface IChannelBroker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="currentAssignedChannels"></param>
        /// <returns></returns>
        List<TwitchChannel> RequestChannelAssignments(string clientId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="currentAssignedChannels"></param>
        /// <returns></returns>
        Task<List<TwitchChannel>> RequestChannelAssignmentsAsync(string clientId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        void ReleaseChannelAssignments(string clientId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task ReleaseChannelAssignmentsAsync(string clientId);
    }
}
