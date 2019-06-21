using ChiBot.TwitchClient.CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.Tests.TwitchClient
{
    public class BotCommandBuilder
    {
        public string User { get; set; } = "user";
        public string Channel { get; set; } = "channel";
        public string Message { get; set; } = "message";
        private const string _IRCBASE = @":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :{2}";

        public BotCommandBuilder WithUser(string user)
        {
            User = user;
            return this;
        }

        public BotCommandBuilder WithChannel(string channel)
        {
            Channel = "#" + channel;
            return this;
        }

        public BotCommandBuilder WithMessage(string message)
        {
            Message = message;
            return this;
        }

        public BotCommand Build()
        {
            return new BotCommand(String.Format(_IRCBASE, User, Channel, Message));
        }
    }
}
