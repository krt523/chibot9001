using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.TwitchClient.Poll
{
    public class PollFactory : IPollFactory
    {
        public IPoll CreatePoll(IEnumerable<string> pollOptions)
        {
            return new Poll(pollOptions);
        }
    }
}
