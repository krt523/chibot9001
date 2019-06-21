using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChiBot.TwitchClient;
using ChiBot.Domain.Models;


namespace ChiBot.TwitchClient.CommandHandler
{
    public interface ICommandHandler
    {
        //Returns true if this ICommandHandler can process this command, else false.
        //Call this method to have the ICommandHandler process this command. True if successful, else false.
        Task<CommandHandlerResult> ProcessCommand(BotCommand command, TwitchChannel channel);
    }
}
