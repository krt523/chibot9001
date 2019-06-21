using System.Collections.Generic;
using ChiBot.TwitchClient.CommandHandler;
using ChiBot.Api.Client;
using ChiBot.Domain.Models;
using System.Threading.Tasks;
using ChiBot.TwitchClient.Poll;
using Microsoft.Practices.Unity;

namespace ChiBot.TwitchClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ClientConfiguration.Instance;

            var container = new UnityContainer();

            container.RegisterInstance(new ChiBotApiClient(new ChiBotHttpClientFactory(configuration.ApiBaseUrl, configuration.ApiUsername, configuration.ApiPassword)));
            container.RegisterInstance(configuration);
            container.RegisterType<IPollFactory, PollFactory>();
            container.RegisterType<IChannelBroker, ChannelBroker>();
            container.RegisterType<ICommandHandler, PollCommandHandler>("Poll");
            container.RegisterType<ICommandHandler, PersonalityCommandHandler>("Personality");
            container.RegisterType<ICommandHandler, QuoteCommandHandler>("Quote");
            container.RegisterType<ICommandHandler, MiscCommandHandler>("Misc");
            container.RegisterInstance(new IrcClient(container.Resolve<ClientConfiguration>(), container.ResolveAll<ICommandHandler>(), container.Resolve<IChannelBroker>()));

            IrcClient client = container.Resolve<IrcClient>();
            client.Start();         
        }
    }
}
