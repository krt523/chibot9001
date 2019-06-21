using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChiBot.Domain;
using ChiBot.Domain.Models;

namespace ChiBot.TwitchClient.CommandHandler.Polling
{
    class PollManager : ICommandHandler
    {

        private readonly List<TwitchChannel> _channels;
        private Dictionary<string, Poll> activePolls;

        public PollManager(List<TwitchChannel> channels) {
            _channels = channels;
            activePolls = new Dictionary<string, Poll>();
        }

        public async Task<bool> ProcessCommand(Command command, IrcClient client)
        {
            switch (command.commandHeader.ToLower())
            {
                case "!startPoll":
                    processStartPoll(command, client);
                    break;
                case "!vote":
                    processVote(command);
                    break;
                case "!endPoll":
                    processEndPoll(command, client);
                    break;
                default:
                    return false;
            }
            return true;
        }

        private void processStartPoll(Command command, IrcClient client) {
            Channel channel = channelRepository.getChannel(command.channel);
            bool atLeastTwoOptions = false;
            if (!activePolls.ContainsKey(command.channel) && channel.administrators.Contains(command.sender))
            {

                List<string> options = command.commandArguments.Split(new char[1] { '!' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                if(options.Count >= 2){
                    activePolls.Add(command.channel, new Poll(options));
                    atLeastTwoOptions = true;
                }
                
                if(atLeastTwoOptions)
                {
                    client.sendPrivmsg(command.channel, "Starting a new poll, use !vote option to vote. Choices are: " + activePolls[command.channel].listOptions());
                } else
                {
                    client.sendPrivmsg(command.channel, "Not enough poll options are present. Ensure the backlog has more than one entry, or multiple options were specified with !{option}");
                }

            } else {
                client.sendPrivmsg(command.channel, "There is already an active poll, or you are not allowed to start a poll.");
            }

        }
        
        private void processVote(Command command) {
            if (activePolls.ContainsKey(command.channel))
            {
                activePolls[command.channel].castVote(command.commandArguments, command.sender);
            }
        }

        private void processEndPoll(Command command, IrcClient client) {
            if (activePolls.ContainsKey(command.channel) && channelRepository.getChannel(command.channel).administrators.Contains(command.sender))
            {
                client.sendPrivmsg(command.channel, activePolls[command.channel].getResults());
                activePolls.Remove(command.channel);
            }
        }
    }
}
