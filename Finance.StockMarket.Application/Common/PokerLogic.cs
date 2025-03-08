using Finance.StockMarket.Domain.PokerGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Common
{
    public static class PokerLogic
    {
        private static readonly Dictionary<string, int> CardValues = new()
    {
        {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6}, {"7", 7}, {"8", 8}, {"9", 9},
        {"10", 10}, {"J", 11}, {"Q", 12}, {"K", 13}, {"A", 14}
    };

        public static string CalculateWinner(Room room)
        {
            if (room == null || room.Players.Count == 0)
                return "No winner, room is empty.";

            var playerScores = new Dictionary<Player, PokerHandRank>();

            foreach (var player in room.Players)
            {
                var allCards = player.Hand.Concat(room.CommunityCards).ToList();
                playerScores[player] = EvaluateHand(allCards);
            }

            var winner = playerScores.OrderByDescending(p => p.Value).First().Key;
            return $"Winner is {winner.Username} with {playerScores[winner]}";
        }

        private static PokerHandRank EvaluateHand(List<Card> cards)
        {
            var groups = cards.GroupBy(c => c.Value)
                              .Select(g => new { Value = g.Key, Count = g.Count() })
                              .OrderByDescending(g => g.Count)
                              .ThenByDescending(g => CardValues[g.Value])
                              .ToList();

            bool isFlush = cards.GroupBy(c => c.Suit).Any(g => g.Count() >= 5);
            bool isStraight = IsStraight(cards.Select(c => c.Value).ToList());

            if (isFlush && isStraight) return PokerHandRank.StraightFlush;
            if (groups[0].Count == 4) return PokerHandRank.FourOfAKind;
            if (groups[0].Count == 3 && groups[1].Count == 2) return PokerHandRank.FullHouse;
            if (isFlush) return PokerHandRank.Flush;
            if (isStraight) return PokerHandRank.Straight;
            if (groups[0].Count == 3) return PokerHandRank.ThreeOfAKind;
            if (groups[0].Count == 2 && groups[1].Count == 2) return PokerHandRank.TwoPair;
            if (groups[0].Count == 2) return PokerHandRank.OnePair;
            return PokerHandRank.HighCard;
        }

        private static bool IsStraight(List<string> values)
        {
            var orderedValues = values.Select(v => CardValues[v]).Distinct().OrderBy(v => v).ToList();
            for (int i = 0; i <= orderedValues.Count - 5; i++)
            {
                if (orderedValues[i + 4] - orderedValues[i] == 4) return true;
            }
            return false;
        }
    }

    public enum PokerHandRank
    {
        HighCard, OnePair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush
    }

}
