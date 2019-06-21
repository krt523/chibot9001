using ChiBot.Api.Client;
using ChiBot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.TwitchClient.CommandHandler
{
    class PersonalityCommandHandler : ICommandHandler
    {
        private readonly ChiBotApiClient _apiClient;
        
        public PersonalityCommandHandler(ChiBotApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CommandHandlerResult> ProcessCommand(BotCommand command, TwitchChannel channel)
        {
            PersonalityCommand personalityCommand;
            string[] args;
            switch (command.Command.ToLower())
            {
                case "!addcommand":
                    //RC1 has some limitations around error handling that make this command able to crash the bot. Since a lot of this is going to get
                    //reworked (maybe) just restrict this command to me for now
                    //only channel admins can run this command.
                    //if(channel.Users.Find( u => u.TwitchUsername == command.sender) == null)
                    if(!(command.Sender == "feelthechi"))
                    {
                        return new CommandHandlerResult(ResultType.Handled);
                    }
                    //split the command arguments, and only use the first two; throw out the rest.
                    args = command.SplitArgumentsOnSpaces(3);
                    personalityCommand = await _apiClient.PersonalityCommands.AddCommandForPersonalityAsync(channel.Personality, new PersonalityCommand(args[0], args[1]));
                    return new CommandHandlerResult(ResultType.HandledWithMessage, $"{personalityCommand.Trigger} was created!", $"#{channel.Name}");

                case "!deletecommand":
                    //only channel admins can run this command.
                    if (channel.Users.Find(u => u.TwitchUsername.ToLower() == command.Sender) == null)
                    {
                        return new CommandHandlerResult(ResultType.Handled);
                    }
                    //split the command arguments, and only use the first one; throw out the rest.
                    args = command.SplitArgumentsOnSpaces(2);
                    await _apiClient.PersonalityCommands.DeleteCommandForPersonalityAsync(channel.Personality, args[0]);
                    return new CommandHandlerResult(ResultType.HandledWithMessage, $"{args[0]} was deleted.", $"#{channel.Name}"); ;

                default:
                    personalityCommand = await _apiClient.PersonalityCommands.GetCommandForPersonalityAsync(channel.Personality, command.Command);
                    if(personalityCommand == null)
                    {
                        return new CommandHandlerResult(ResultType.NotHandled);
                    } else
                    {
                        return new CommandHandlerResult(ResultType.HandledWithMessage, personalityCommand.Response, $"#{channel.Name}"); ;
                    }
            }
        }
    }
}
