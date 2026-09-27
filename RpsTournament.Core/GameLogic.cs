using System;

namespace RpsTournament.Core
{
    public static class GameLogic
    {
        private static readonly Random RandomGenerator = new Random();

        public static Move GetRandomComputerMove()
        {
            Move[] moves = new Move[] { Move.Rock, Move.Paper, Move.Scissors };
            int index = RandomGenerator.Next(moves.Length);
            return moves[index];
        }

        public static RoundResult EvaluateRound(Move playerMove, Move computerMove)
        {
            if (playerMove == computerMove)
                return RoundResult.Draw;

            switch (playerMove)
            {
                case Move.Rock:
                    return (computerMove == Move.Scissors) ? RoundResult.PlayerWin : RoundResult.ComputerWin;
                case Move.Paper:
                    return (computerMove == Move.Rock) ? RoundResult.PlayerWin : RoundResult.ComputerWin;
                case Move.Scissors:
                    return (computerMove == Move.Paper) ? RoundResult.PlayerWin : RoundResult.ComputerWin;
                default:
                    return RoundResult.Draw;
            }
        }

        public static bool ValidatePlayerName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            int length = name.Trim().Length;
            return length >= 2 && length <= 30;
        }
    }
}