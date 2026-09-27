namespace RpsTournament.Core
{
    public struct GameRound
    {
        public int RoundNumber { get; set; }
        public string PlayerName { get; set; }
        public Move PlayerMove { get; set; }
        public Move ComputerMove { get; set; }
        public RoundResult Result { get; set; }
        public string ResultText { get; set; }
    }
}