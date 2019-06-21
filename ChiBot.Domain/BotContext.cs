using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChiBot.Domain.Models;

namespace ChiBot.Domain
{
    public class BotContext : DbContext
    {
        public DbSet<Personality> Personalities { get; set; }
        public DbSet<TwitchChannel> TwitchChannels { get; set; }
        public DbSet<PersonalityCommand> PersonalityCommands { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DiscordGuild> DiscordGuilds { get; set; }
    }
}
