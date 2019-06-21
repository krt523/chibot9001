using NUnit.Framework;
using ChiBot.TwitchClient.Poll;

namespace ChiBot.Tests.TwitchClient
{
    [TestFixture]
    public class PollTests
    {

        [Test]
        public void CastVote_CastTwoVotesForOneOption_CastVoteReturnsTwo()
        {
            //Arrange
            var poll = new Poll(new string[] { "option 1", "option 2" });

            //Act
            poll.CastVote("Voter 1", 1);
            int voteCount = poll.CastVote("Voter 2", 1);

            //Assert
            Assert.AreEqual(2, voteCount);
        }

        [Test]
        public void CastVote_CastVoteForTwoOptions_CastVoteReturnsOneForEach()
        {
            //Arrange
            var poll = new Poll(new string[] { "option 1", "option 2" });

            //Act
            int voteCount1 = poll.CastVote("Voter 1", 1);
            int voteCount2 = poll.CastVote("Voter 2", 2);

            //Assert
            Assert.AreEqual(voteCount1, 1);
            Assert.AreEqual(voteCount2, 1);
        }

        [Test]
        public void CastVote_CastVoteOutOfBounds_CastVoteReturnsNegativeOne()
        {
            //Arrange
            var poll = new Poll(new string[] { "option 1", "option 2" });

            //Act
            int voteCount = poll.CastVote("Voter 1", 0);

            //Assert
            Assert.AreEqual(voteCount, -1);
        }

        [Test]
        public void CastVote_VoterCastsTwoVotes_OneVoteCounts()
        {
            //Arrange
            var poll = new Poll(new string[] { "option 1", "option 2" });

            //Act
            int voteCount1 = poll.CastVote("Voter 1", 1);
            int voteCount2 = poll.CastVote("Voter 1", 1);

            //Assert
            Assert.AreEqual(voteCount1, 1);
            Assert.AreEqual(voteCount2, -1);
        }

        [Test]
        public void ViewResults_PollWithVotes_ResultsAreDisplayed()
        {
            //Arrange
            var poll = new Poll(new string[] { "Monster Hunter World", "DmC", "DbD" });
            poll.CastVote("Voter 1", 1);
            poll.CastVote("Voter 2", 1);
            poll.CastVote("Voter 3", 1);
            poll.CastVote("Voter 4", 1);
            poll.CastVote("Voter 5", 2);
            poll.CastVote("Voter 6", 2);

            //Act
            string result = poll.EndPoll();

            //Assert
            Assert.AreEqual(result, "Poll results are in!!! 1. Monster Hunter World:4 -- 2. DmC:2 -- 3. DbD:0");
        }
    }
}
