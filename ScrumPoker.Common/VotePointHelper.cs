using ScrumPoker.Exceptions;
using ScrumPoker.Model.Enums;
using ScrumPoker.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrumPoker.Common
{
    public class VotePointHelper
    {
        public static CardPoints GetNearestCard(List<CardPoints> selectedCards)
        {
            if (!selectedCards.Any())
                return CardPoints.Zero;

            var groupResult = selectedCards.GroupBy(g => g).Select(grp => new
            {
                value = grp.Key,
                count = grp.Count()
            }).ToList();

            var mostSelectedCard = groupResult.OrderByDescending(p => p.count).First().value;
            if ((int)mostSelectedCard < 0)
                return mostSelectedCard;

            var avarage = Convert.ToDecimal(selectedCards.Any() ? selectedCards.Average(p => (int)p) : 0);
            var fibonacciList = Enum.GetValues(typeof(CardPoints)).Cast<CardPoints>().ToList();
            return fibonacciList.OrderBy(x => Math.Abs((int)x - avarage)).First();
        }

        public static string GetBestPredictionUsers(List<JiraTask> taskList)
        {
            if (taskList == null || taskList.Count == 0)
                throw new PredictionException("The most successful estimates could not be calculated because the task list was empty.");

            List<UserVote> list = new List<UserVote>();

            foreach (var task in taskList)
                foreach (var userVote in task.UserVotes)
                    if ((int)task.ComfirmedPoint == userVote.Vote)
                        list.Add(userVote);

            var result = list.GroupBy(g => g.Username).Select(group => new
            {
                Name = group.Key,
                Count = group.Count()
            });

            var max = result.Max(m => m.Count);

            var winners = result.Where(p => p.Count == max).Select(s => s.Name);

            if(!winners.Any())
                throw new PredictionException("There are no participants who guessed correctly.");

            return $"Most successful voter(s) : {string.Join(", ", winners)}";
        }
    }
}
