using System;
using System.Collections.Generic;


namespace ChiBot.CommandHandler.Polling
{
    public class Poll
    {
        List<PollItem> pollItems;
        List<string> voters;

        public Poll(List<string> pollOptions) {
            pollItems = new List<PollItem>();
            foreach(string option in pollOptions)
            {
                pollItems.Add(new PollItem(option.Trim()));
            }
            voters = new List<string>();
        }

        /// <summary>
        /// Casts a vote for the provided poll option so long as the voter has not already voted and the option exists in the poll. 
        /// </summary>
        /// <param name="option">The option to cast a vote for.</param>
        /// <param name="voter">The name of the user casting the vote.</param>
        public void castVote(string option, string voter) {

            foreach(PollItem item in pollItems)
            {
                if(item.optionName.Equals(option.Trim()) && !voters.Contains(voter))
                {
                    item.numberOfVotes++;
                    voters.Add(voter);
                }
            }
        }

        /// <summary>
        /// Displays the current result of the poll. Prints the winning options first.
        /// </summary>
        /// <returns>String representation of the results.</returns>
        public string getResults() {
            sortResults();
            string result = pollItems[0].optionName + ": " + pollItems[0].numberOfVotes;
            for(int i = 1; i <= pollItems.Count - 1; i++)
            {
                result = result + " -- " + pollItems[i].optionName + ": " + pollItems[i].numberOfVotes;
            }
            return result;
        }

        public string listOptions() {
            string result = pollItems[0].optionName;
            for (int i = 1; i <= pollItems.Count - 1; i++)
            {
                result = result + " -- " + pollItems[i].optionName;
            }
            return result;
        }

        /// <summary>
        /// Gets the number of options in the poll.
        /// </summary>
        /// <returns>int value of the number of options</returns>
        public int getOptionCount() {
            return this.pollItems.Count;
        }

        /// <summary>
        /// Uses an insertion sort to sort the current PollOptions list so higher votes are at the top of the list.
        /// </summary>
        private void sortResults() {
            for(int i = 1; i < (pollItems.Count - 1); i++)
            {
                int j = i;
                while(j > 0 && pollItems[j-1].numberOfVotes > pollItems[j].numberOfVotes)
                {
                    PollItem temp = pollItems[j];
                    pollItems[j] = pollItems[j - 1];
                    pollItems[j - 1] = temp;
                    j--; 
                }
            }
        }
    }
}
