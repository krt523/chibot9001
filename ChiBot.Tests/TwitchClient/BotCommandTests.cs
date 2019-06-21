using NUnit.Framework;
using ChiBot.TwitchClient.CommandHandler;


namespace ChiBot.Tests.TwitchClient
{
    [TestFixture]
    class BotCommandTests
    {
        private string IRCLine = @":feelthechi!feelthechi@feelthechi.tmi.twitch.tv PRIVMSG #tminator64 :!quote 5 6 7 8";

        [Test]
        public void CreateBotCommand_ConstructorPassedIRCLine_BotCommandCreatedCorrecty()
        {
            //Arrange
            var command = new BotCommand(IRCLine);

            //Act


            //Assert
            Assert.AreEqual("feelthechi", command.Sender);
            Assert.AreEqual("!quote", command.Command);
            Assert.AreEqual("5 6 7 8", command.Arguments);
            Assert.AreEqual("#tminator64", command.Channel);
        }

        [Test]
        public void SplitArguments_StringOfArguments_ArrayOfArgumentsReturned()
        {
            //Arrange
            string[] expected = { "5", "6", "7 8" };
            var command = new BotCommand(IRCLine);

            //Act
            string[] actual = command.SplitArgumentsOnSpaces(3);

            //Assert
            Assert.AreEqual(actual.Length, 3);
            for(int i = 0; i < 3; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
