using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.CommandHandler.Quotes
{
    class QuoteList {
        public List<string> quotes;
        public string channel;
        public int index;

        public QuoteList(string channel, List<string> quotes) {
            this.channel = channel;
            this.quotes = quotes;
            index = 0;
        }

        public string getNextQuote() {
            if(quotes.Count == 0)
            {
                return "No quotes for this channel.";
            } else {
                return quotes[new Random().Next(0, (quotes.Count -1))];
            }

        }

        public string getSpecificQuote(int quoteNumber) {
            try
            {
                return quotes[quoteNumber];
            } catch (ArgumentOutOfRangeException)
            {
                return "There is no quote "+ quoteNumber +". If you want that many quotes make the streamer say more funny things.";
            }
        }
    }
}
