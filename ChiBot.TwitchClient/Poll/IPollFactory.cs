using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.TwitchClient.Poll
{
    public interface IPollFactory
    {
        IPoll CreatePoll(IEnumerable<string> pollOptions);
    }
}
