using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiBot.TwitchClient.CommandHandler
{
    public enum ResultType { HandledWithMessage, Handled, NotHandled};

    public class CommandHandlerResult
    {
        public ResultType ResultType { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }

        public CommandHandlerResult(ResultType resultType)
        {
            ResultType = resultType;
            Message = null;
            Channel = null;
        }

        public CommandHandlerResult(ResultType resultType, string message, string channel)
        {
            ResultType = resultType;
            Message = message;
            Channel = channel;
        }
    }


}
