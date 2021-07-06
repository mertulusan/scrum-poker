using ScrumPoker.Model.Enums;
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

            var groupResult = selectedCards.GroupBy(g => g).Select(grp => new {
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
    }
}
