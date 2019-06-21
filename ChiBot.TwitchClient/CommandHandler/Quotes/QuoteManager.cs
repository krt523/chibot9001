using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChiBot.TwitchClient;
using ChiBot.TwitchClient.CommandHandler;
using ChiBot.Domain;


namespace ChiBot.CommandHandler.Quotes
{
    class QuoteManager : ICommandHandler
    {
        private IQuoteRepository quoteRepository;
        private List<QuoteList> quoteLists;
        private IChannelRepository channelRepository;

        public QuoteManager(IQuoteRepository quoteRepository, IChannelRepository channelRepository) {
            this.quoteRepository = quoteRepository;
            this.channelRepository = channelRepository;
            quoteLists = new List<QuoteList>();
        }

        public bool isValidCommand(Command command) {
            throw new NotImplementedException();
        }

        public bool ProcessCommand(Command command, IrcClient client) {
            QuoteList currentlist;
            switch (command.commandHeader)
            {
                case "!quote":
                    //find the quote list for the channel the command came from
                   currentlist = quoteLists.SingleOrDefault(x => x.channel == command.channel);
                    //if we don't have a quote list for the channel, load it.
                    if(currentlist == null)
                    {
                        currentlist = new QuoteList(command.channel, quoteRepository.getQuotes(command.channel));
                        quoteLists.Add(currentlist);
                    }
                    //send quote.
                    if (String.IsNullOrEmpty(command.commandArguments))
                    {
                        client.sendPrivmsg(command.channel, currentlist.getNextQuote());
                    } else
                    {
                        try
                        {
                            int quoteNum = Int32.Parse(command.commandArguments);
                            if(quoteNum < 0)
                            {
                                throw new FormatException();
                            }
                            client.sendPrivmsg(command.channel, currentlist.getSpecificQuote(quoteNum));
                        } catch (FormatException)
                        {
                            client.sendPrivmsg(command.channel, "Are you drunk, " + command.sender + "? " + command.commandArguments + " isn't a valid positive integer. This is why I hate humans. You can't even submit valid input!");
                            return true;
                        } catch (OverflowException)
                        {
                            client.sendPrivmsg(command.channel, "I doubt you'd have that many quotes if you added up the saved quotes of every bot on twitch... Such a silly human.");
                            return true;
                        }
                        
                    }
                    
                    return true;

                case "!addquote":
                    if (channelRepository.getChannel(command.channel).administrators.Contains(command.sender))
                    {
                        //find the quote list for the channel the command came from.
                        currentlist = quoteLists.SingleOrDefault(x => x.channel == command.channel);
                        //if we don't have a quote list for the channel, load it.
                        if (currentlist == null)
                        {
                            currentlist = new QuoteList(command.channel, quoteRepository.getQuotes(command.channel));
                            quoteLists.Add(currentlist);
                        }
                        //add the quote to the currently loaded list, save the new list of quotes.
                        currentlist.quotes.Add(command.commandArguments);
                        quoteRepository.saveQuotes(command.channel, currentlist.quotes);
                        client.sendPrivmsg(command.channel, "Added quote: " + command.commandArguments); 
                    } else
                    {
                        client.sendPrivmsg(command.channel, "You must be a bot administrator to add a quote.");
                    }
                    return true;
                default:
                    return false;
            }
        }
    }
}
