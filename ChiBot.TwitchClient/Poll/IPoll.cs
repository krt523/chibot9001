using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.TwitchClient.Poll
{
    public interface IPoll
    {
        int CastVote(string voter, int optionNumber);
        string EndPoll();
        string PrintPoll();
    }
}
