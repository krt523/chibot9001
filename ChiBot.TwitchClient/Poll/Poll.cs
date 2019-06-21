using System;
using System.Collections.Generic;

namespace ChiBot.TwitchClient.Poll
{
    public class Poll : IPoll
    {
        public HashSet<string> Voters{ get; private set;}
        private List<PollOption> _pollOptions; 

        public Poll(IEnumerable<string> pollOptions)
        {
            _pollOptions = new List<PollOption>();
            Voters = new HashSet<string>();
            foreach(string option in pollOptions)
            {
                _pollOptions.Add(new PollOption(option));
            }
        }
       
        public int CastVote(string voter, int optionNumber)
        {
            if (Voters.Add(voter))
            {
                try
                {
                    return _pollOptions[optionNumber - 1].AddVote();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return -1;
                }
            } else
            {
                return -1;
            }  
        }

        public string EndPoll()
        {
            _pollOptions.Sort(PollOption.CompareByVoteTotalDescending);
            return $"Poll results are in!!! {this.PrintPoll()}";
        }

        public string PrintPoll()
        {
            //start string
            string optionsToString = $"1. {_pollOptions[0].ToString()}";

            //build the rest
            for(int i = 1; i < _pollOptions.Count; i++)
            {
                optionsToString += $" -- {i + 1}. {_pollOptions[i].ToString()}";
            }

            return optionsToString.Trim();
        }
        
        private class PollOption
        {
            public string OptionValue { get; private set; }
            public int VoteTotal { get; private set; }

            public static int CompareByVoteTotalDescending(PollOption x, PollOption y)
            {
                if(x.VoteTotal > y.VoteTotal)
                {
                    return -1;
                } else if(x.VoteTotal < y.VoteTotal) {
                    return 1;
                } else
                {
                    return 0;
                }
            }

            public PollOption(string optionValue)
            {
                OptionValue = optionValue.Trim();
                VoteTotal = 0;
            }

            public int AddVote()
            {
                VoteTotal++;
                return VoteTotal;
            }

            public override string ToString()
            {
                return $"{OptionValue}:{VoteTotal}";
            }
        }        
    }
}