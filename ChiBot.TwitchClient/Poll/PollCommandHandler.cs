using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChiBot.TwitchClient.CommandHandler;
using ChiBot.Domain.Models;

namespace ChiBot.TwitchClient.Poll
{
    public class PollCommandHandler : ICommandHandler
    {
        private Dictionary<string, IPoll> _activePolls;
        private List<TwitchChannel> _channels;
        private IPollFactory _pollFactory;

        public PollCommandHandler(IPollFactory pollFactory)
        {
            _activePolls = new Dictionary<string, IPoll>();
            _pollFactory = pollFactory;
        }

        public Task<CommandHandlerResult> ProcessCommand(BotCommand command, TwitchChannel channel)
        {

            switch (command.Command)
            {
                case "!poll":
                    if(AuthorizeCommand(command.Sender, channel))
                    {
                        if (!_activePolls.ContainsKey(channel.Name))
                        {
                            string[] optionArray = command.Arguments.Split(new char[] { '!' }, StringSplitOptions.RemoveEmptyEntries);
                            optionArray = optionArray.Select(x => x.Trim()).ToArray();
                            IPoll poll = _pollFactory.CreatePoll(optionArray);
                            _activePolls.Add(channel.Name, poll);
                            return Task.FromResult(new CommandHandlerResult(ResultType.HandledWithMessage,
                                $"Poll started! use !vote <number> to case your vote. {poll.PrintPoll()}",
                                $"#{channel.Name}"));
                        }
                        else
                        {
                            IPoll poll = _activePolls[channel.Name];
                            string pollResult = poll.EndPoll();
                            _activePolls.Remove(channel.Name);
                            return Task.FromResult(new CommandHandlerResult(ResultType.HandledWithMessage,
                                pollResult,
                                $"#{channel.Name}"));
                        }
                    } else
                    {
                        //Even if we've done nothing, the poll command has been handled.
                        return Task.FromResult(new CommandHandlerResult(ResultType.Handled));
                    }
                case "!vote":
                    if (_activePolls.ContainsKey(channel.Name))
                    {
                        if (Int32.TryParse(command.SplitArgumentsOnSpaces(1)[0], out int votersChoice))
                        {
                            _activePolls[channel.Name].CastVote(command.Sender, votersChoice);
                        }
                        
                    }
                    //Even if we've done nothing, the vote command has been handled.
                    return Task.FromResult(new CommandHandlerResult(ResultType.Handled));
                default:
                    return Task.FromResult(new CommandHandlerResult(ResultType.NotHandled));
            }
        }

        private bool AuthorizeCommand(string username, TwitchChannel twitchChannel)
        {
            if(twitchChannel.Users.Find(u => u.TwitchUsername.ToLower() == username) == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
