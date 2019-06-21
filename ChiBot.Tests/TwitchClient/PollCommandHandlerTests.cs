using NUnit.Framework;
using ChiBot.Domain.Models;
using ChiBot.TwitchClient.Poll;
using ChiBot.TwitchClient.CommandHandler;
using Moq;
using System.Collections.Generic;

namespace ChiBot.Tests.TwitchClient
{
    [TestFixture]
    class PollCommandHandlerTests
    {
        private const string _ADMINUSER = "adminuser";
        private TwitchChannel _channel;
        
        [SetUp]
        protected void SetUp()
        {
            var user = new User() { TwitchUsername = "adminuser" };
            _channel = new TwitchChannel() { Users = new List<User>() { user }, Name = "channel" };
        }

        [Test]
        public void ProcessCommand_NoOpenPoll_NewPollCreated()
        {
            //Arrange
            var command = new BotCommandBuilder().WithUser(_ADMINUSER).WithMessage("!poll !option 1 !option 2 !option 3").Build();

            var poll = new Mock<IPoll>();
            poll.Setup(x => x.PrintPoll()).Returns("Print Poll");

            var pollFacotry = new Mock<IPollFactory>();
            pollFacotry.Setup(x => x.CreatePoll(new string[] {"option 1", "option 2", "option 3" })).Returns(poll.Object);

            var pollCommandHandler = new PollCommandHandler(pollFacotry.Object);
            //Act
            CommandHandlerResult result = pollCommandHandler.ProcessCommand(command, _channel).Result;

            //Assert
            Assert.AreEqual(ResultType.HandledWithMessage, result.ResultType);
            Assert.AreEqual($"Poll started! use !vote <number> to case your vote. Print Poll", result.Message);
            Assert.AreEqual("#channel", result.Channel);
        }

        [Test]
        public void ProcessCommand_PollOpen_PollClosed()
        {
            //Arrange
            BotCommand command = new BotCommandBuilder().WithUser(_ADMINUSER).WithMessage("!poll !option 1 !option 2 !option 3").Build();
            BotCommand command2 = new BotCommandBuilder().WithUser(_ADMINUSER).WithMessage("!poll").Build();

            var poll = new Mock<IPoll>();
            poll.Setup(x => x.EndPoll()).Returns("End Poll");

            var pollFacotry = new Mock<IPollFactory>();
            pollFacotry.Setup(x => x.CreatePoll(new string[] { "option 1", "option 2", "option 3" })).Returns(poll.Object);

            var pollCommandHandler = new PollCommandHandler(pollFacotry.Object);
            //Act
            CommandHandlerResult result = pollCommandHandler.ProcessCommand(command, _channel).Result;
            result = pollCommandHandler.ProcessCommand(command2, _channel).Result;


            //Assert
            Assert.AreEqual(ResultType.HandledWithMessage, result.ResultType);
            Assert.AreEqual($"End Poll", result.Message);
            Assert.AreEqual("#channel", result.Channel);

        }

        [Test]
        public void ProcessCommand_UnauthorizedUserCreatesPoll_ResultTypeIsHandled()
        {
            //Arrange
            BotCommand command = new BotCommandBuilder().WithUser("bob").WithMessage("!poll !option 1 !option 2 !option 3").Build();

            var poll = new Mock<IPoll>();
            var pollFacotry = new Mock<IPollFactory>();

            var pollCommandHandler = new PollCommandHandler(pollFacotry.Object);
            //Act
            CommandHandlerResult result = pollCommandHandler.ProcessCommand(command, _channel).Result;

            //Assert
            Assert.AreEqual(ResultType.Handled, result.ResultType);
            Assert.AreEqual(null, result.Message);
            Assert.AreEqual(null, result.Channel);
        }

        [Test]
        public void ProcessCommand_UserCastsVote_VoteIsCast()
        {
            //Arrange
            BotCommand command = new BotCommandBuilder().WithUser(_ADMINUSER).WithMessage("!poll !option 1 !option 2 !option 3").Build();
            BotCommand command2 = new BotCommandBuilder().WithUser("user").WithMessage("!vote 2").Build();

            var poll = new Mock<IPoll>();
            poll.Setup(x => x.CastVote("user", 2)).Returns(1);

            var pollFacotry = new Mock<IPollFactory>();
            pollFacotry.Setup(x => x.CreatePoll(new string[] { "option 1", "option 2", "option 3" })).Returns(poll.Object);

            var pollCommandHandler = new PollCommandHandler(pollFacotry.Object);

            //Act
            CommandHandlerResult result = pollCommandHandler.ProcessCommand(command, _channel).Result;
            result = pollCommandHandler.ProcessCommand(command2, _channel).Result;

            //Assert
            poll.Verify(x => x.CastVote("user", 2), Times.Once());
            Assert.AreEqual(ResultType.Handled, result.ResultType);
            Assert.AreEqual(null, result.Message);
            Assert.AreEqual(null, result.Channel);
        }
    }
}
