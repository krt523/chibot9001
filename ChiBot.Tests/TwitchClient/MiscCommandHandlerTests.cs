using NUnit.Framework;
using ChiBot.Domain.Models;
using ChiBot.TwitchClient.CommandHandler;
using Moq;
using System.Collections.Generic;
using System;

namespace ChiBot.Tests.TwitchClient
{
    [TestFixture]
    class MiscCommandHandlerTests
    {
        private const string _ADMINUSER = "adminuser";
        private TwitchChannel _channel;
        private MiscCommandHandler _commandHandler;

        [SetUp]
        protected void SetUp()
        {
            var user = new User() { TwitchUsername = "adminuser" };
            _channel = new TwitchChannel() { Users = new List<User>() { user }, Name = "channel" };
            _commandHandler = new MiscCommandHandler();
        }

        [Test]
        public void ProcessCommand_Roll_ValidInputNoModifier()
        {
            //Arrange
            var command = new BotCommandBuilder().WithMessage("!roll 1d4").Build();

            //Act
            CommandHandlerResult result = _commandHandler.ProcessCommand(command, _channel).Result;

            //Assert
            Assert.AreEqual(ResultType.HandledWithMessage, result.ResultType);
            Assert.GreaterOrEqual(Int32.Parse(result.Message), 1);
            Assert.LessOrEqual(Int32.Parse(result.Message), 4);
        }

        [Test]
        public void ProcessCommand_Roll_ValidInputPositiveModifier()
        {
            //Arrange
            var command = new BotCommandBuilder().WithMessage("!roll 1d4+2").Build();

            //Act
            CommandHandlerResult result = _commandHandler.ProcessCommand(command, _channel).Result;

            //Assert
            Assert.GreaterOrEqual(Int32.Parse(result.Message), 3);
            Assert.LessOrEqual(Int32.Parse(result.Message), 6);
        }

        [Test]
        public void ProcessCommand_Roll_ValidInputNegativeModifier()
        {
            //Arrange
            var command = new BotCommandBuilder().WithMessage("!roll 2d6-2").Build();

            //Act
            CommandHandlerResult result = _commandHandler.ProcessCommand(command, _channel).Result;

            //Assert
            Assert.GreaterOrEqual(Int32.Parse(result.Message), 0);
            Assert.LessOrEqual(Int32.Parse(result.Message), 10);
        }

        [Test]
        public void ProcessCommand_8_ValidQuestionAsked()
        {
            //Arrange
            var command = new BotCommandBuilder().WithMessage("!8 a question?").Build();

            //Act
            CommandHandlerResult result = _commandHandler.ProcessCommand(command, _channel).Result;

            //Assert
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(ResultType.HandledWithMessage, result.ResultType);
        }

        [Test]
        public void ProcessCommand_8_NoQuestionAsked()
        {
            //Arrange
            var command = new BotCommandBuilder().WithMessage("!8  ").Build();

            //Act
            CommandHandlerResult result = _commandHandler.ProcessCommand(command, _channel).Result;

            //Assert
            Assert.IsNotNull(result.Message);
            Assert.AreEqual(ResultType.HandledWithMessage, result.ResultType);
        }
    }
}
