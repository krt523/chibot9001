using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChiBot.Api.Client;
using ChiBot.Domain.Models;

namespace ChiBot.TwitchClient.CommandHandler
{
    class QuoteCommandHandler : ICommandHandler
    {
        private readonly ChiBotApiClient _apiClient;

        public QuoteCommandHandler(ChiBotApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CommandHandlerResult> ProcessCommand(BotCommand command, TwitchChannel channel)
        {
            Quote result;
            string[] args;
            switch (command.Command.ToLower())
            {
                case "!quote":
                    args = command.SplitArgumentsOnSpaces(2);

                    //check to see if the command was passed arguments.
                    if(args.Length > 0)
                    {
                        bool hasQuoteNumber;
                        int quoteNumber;

                        // attempt to turn the first argument recieved into an int index to get a specific quote.
                        hasQuoteNumber = Int32.TryParse(args[0], out quoteNumber);
                        if (hasQuoteNumber)
                        {
                            //attempt to get a specificquote; otherwise return a message indicating failure. 
                            result = await _apiClient.Quotes.GetSpecificQuoteForPersonalityAsync(channel.Personality, quoteNumber);
                            string responseMessage;
                            if (result == null)
                                responseMessage = $"Quote number {quoteNumber} doesn't exist. Beg the bot admins to add more quotes.";
                            else
                                responseMessage = result.Content;
                            return new CommandHandlerResult(ResultType.HandledWithMessage, responseMessage, $"#{channel.Name}");
                        }
                    }
                    //no args or bad args passed; return a random quote for the personality
                    result = await _apiClient.Quotes.GetRandomQuoteForPersonalityAsync(channel.Personality);
                    return new CommandHandlerResult(ResultType.HandledWithMessage, result.Content, $"#{channel.Name}");

                case "!addquote":
                    //only channel admins can run this command
                    if (channel.Users.Find(u => u.TwitchUsername.ToLower() == command.Sender) == null)
                    {
                        return new CommandHandlerResult(ResultType.Handled);
                    } else
                    {
                        result = await _apiClient.Quotes.AddQuoteForPersonalityAsync(channel.Personality, command.Arguments);
                        return new CommandHandlerResult(ResultType.HandledWithMessage, result.Content, $"#{channel.Name}");
                    }

                default:
                    return new CommandHandlerResult(ResultType.NotHandled);
            }
        }
    }
}
