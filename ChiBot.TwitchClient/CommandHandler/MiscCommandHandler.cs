using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChiBot.Domain.Models;

namespace ChiBot.TwitchClient.CommandHandler
{
    public class MiscCommandHandler : ICommandHandler
    {
        private static string[] _RESPONSES = new string[] {"It is certain", "It is decidedly so", "Without a doubt",
                            "Yes definitely", "You may rely on it", "As I see it, yes", "Most likely",
                            "Outlook good", "Yes", "Signs point to yes", "Reply hazy try again", "Ask again later",
                            "Better not tell you now", "Cannot predict now", "Concentrate and ask again", "Don't count on it",
                            "My reply is no", "My sources say no", "Outlook not so good", "Very doubtful"};

        public Task<CommandHandlerResult> ProcessCommand(BotCommand command, TwitchChannel channel)
        {
            string channelName = $"#{channel.Name}";
            //check other misc commands.
            switch (command.Command)
            {
                case "!roll":
                    //dice roll command is in form of !roll nDm+x, where n is number of dice, m is dice size, and x is the modifier. 
                    Regex rollRegex = new Regex(@"(\d*)?[Dd](\d*)(\+\d*|-\d*)?");
                    Match match = rollRegex.Match(command.Arguments);
                    if (match.Success)
                    {
                        return Task.FromResult(DiceRollCommand(match, channelName));
                    } else
                    {
                        return Task.FromResult(new CommandHandlerResult(ResultType.NotHandled));
                    }
                case "!8":
                    return Task.FromResult(MagicEightBallCommand(command, channelName));
                default:
                    return Task.FromResult(new CommandHandlerResult(ResultType.NotHandled));
            }
        }

        private CommandHandlerResult MagicEightBallCommand(BotCommand command, string channelName)
        {
            if (!string.IsNullOrEmpty(command.Arguments))
            {
                return new CommandHandlerResult(ResultType.HandledWithMessage, _RESPONSES[new Random().Next(0, _RESPONSES.Length - 1)], channelName);
            }
            return new CommandHandlerResult(ResultType.HandledWithMessage, "I'm a bot, not a mind reader. Ask a question.", channelName);
        }

        private CommandHandlerResult DiceRollCommand(Match match, string channelName)
        {

            if (!match.Groups[1].Success || !Int32.TryParse(match.Groups[1].Value, out int numberOfDice))
            {
                numberOfDice = 1;
            }

            var allowedDiceSizes = new List<int>() { 4, 6, 10, 12, 20, 100 };
            if (!match.Groups[2].Success || !Int32.TryParse(match.Groups[2].Value, out int diceSize) || !allowedDiceSizes.Contains(diceSize))
            {
                return new CommandHandlerResult(ResultType.Handled);
            }

            if (!match.Groups[3].Success || !Int32.TryParse(match.Groups[3].Value, out int modifier))
            {
                modifier = 0;
            }

            int rollResult = numberOfDice * diceSize + modifier;

            return new CommandHandlerResult(ResultType.HandledWithMessage, rollResult.ToString(), channelName);
        }
    }
}
