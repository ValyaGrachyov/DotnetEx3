namespace Domain.TicTacToe.GameEvents
{
    public class TurnSwitchEvent : TicTacToeGameEvent
    {
        public string WaitingForUserId { get; set; }
        public string WaitingForUser { get; set; }

    }
}
