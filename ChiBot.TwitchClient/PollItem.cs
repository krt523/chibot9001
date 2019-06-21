using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.CommandHandler.Polling
{
    class PollItem
    {
        public string optionName { get; }
        public int numberOfVotes { get; set; }

        public PollItem(string optionName) {
            this.optionName = optionName;
            this.numberOfVotes = 0;
        }
    }
}
