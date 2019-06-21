using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChiBot.TwitchClient.CommandHandler
{
    public class BotCommand
    {
        public string Sender { get; set; }
        public string Channel { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }

        public BotCommand(string twitchIrcLine) {
            Regex commandRegex = new Regex(@"^:(\w+)!.+(PRIVMSG)\s(#\w+)\s:(!\w+)\s?(.*)");
            Match match = commandRegex.Match(twitchIrcLine);
            this.Sender = match.Groups[1].Value;
            this.Channel = match.Groups[3].Value;
            this.Command = match.Groups[4].Value;
            this.Arguments = match.Groups[5].Value;
        }

        public string[] SplitArgumentsOnSpaces(int numberOfArgs)
        {
            return Arguments.Split(new char[] { ' ', '\t' }, numberOfArgs);
        }

        public static bool isPotentialCommand(string twitchIrcLine) {

            if (twitchIrcLine.Contains("PRIVMSG") && twitchIrcLine.Contains(":!"))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
